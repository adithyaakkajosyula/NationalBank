using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NationalBank.BackEnd.Helpers;
using NationalBank.BackEnd.Authorization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using NationalBank.BackEnd.Entities;
using NationalBank.Api.Filters;
using Serilog;
using System.Threading.RateLimiting;
using NationalBank.BackEnd.Configuration;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using NationalBank.Api.Controllers;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var env = builder.Environment;

var logFilePath = configuration["Serilog:WriteTo:0:Args:path"]; // Read path from config

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Error()
    .Enrich.FromLogContext();

if (env.IsDevelopment())
{
    // Use file path from appsettings.json
    loggerConfig.WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day);
}
else
{
    // Use Azure Blob in production
    loggerConfig.WriteTo.AzureBlobStorage(
        connectionString: "DefaultEndpointsProtocol=https;AccountName=nationalbank;AccountKey=F5Q7h+Jo6Kg8MNsQXd+ZmqULeIqIJ5J45332d4kIXDT/EEp6U0dJAKnFkLKesztI7oGVAp30CCoL+AStL2MAgg==;EndpointSuffix=core.windows.net",
        storageContainerName: "logs",
        storageFileName: "ApiLog.json",
        restrictedToMinimumLevel: LogEventLevel.Error
    );
}

Log.Logger = loggerConfig.CreateLogger();

builder.Host.UseSerilog();

// your EDM model
// OData model
IEdmModel GetEdmModel()
{
    var osv = new ODataConventionModelBuilder();
    osv.EntitySet<Employee>("Employees");    // <-- entity set name
    return osv.GetEdmModel();
}

builder.Services
    .AddControllers()
    .AddOData(opt => opt
        .AddRouteComponents("odata", GetEdmModel()) 
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(100)
        .EnableQueryFeatures());                   

builder.Services.AddControllers().AddXmlSerializerFormatters()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        /*options.ApiVersionReader = ApiVersionReader.Combine(
             new UrlSegmentApiVersionReader(),
             new HeaderApiVersionReader("api-version"));*/
        //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
        // options.ApiVersionReader = new HeaderApiVersionReader("api-version"); 

        options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });


builder.Services.AddEndpointsApiExplorer();
//For OData
builder.Services.AddSwaggerGen(o =>
{
    o.OperationFilter<ODataQueryOptionsOperationFilter>();
});
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();


/*builder.Services.AddInMemoryRateLimiting();

builder.Services.Configure<IpRateLimitOptions>(
        builder.Configuration.GetSection("IpRateLimiting"));

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();*/

//   RATE-LIMITER CONFIG 
builder.Services.AddRateLimiter(options =>
{
    // Global Concurrency Limiter: 2 running, 50 waiting
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
        RateLimitPartition.GetConcurrencyLimiter(
            partitionKey: "global",
            _ => new ConcurrencyLimiterOptions
            {
                PermitLimit = 1,            // active slots
                QueueLimit = 1,            // queued requests
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));

    // Return 429 instead of default 503
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Custom body + logging when blocked
    options.OnRejected = async (context, ct) =>
    {
        var log = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                       .CreateLogger("RateLimiter");
        log.LogWarning(" Rejected {Path} – queue full", context.HttpContext.Request.Path);

        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(
            """{ "error": "Too many concurrent requests" }""", ct);
    };
});

builder.Services.AddScoped<APIIActionFilter>();
builder.Services.AddScoped<APIIExceptionFilter>();
builder.Services.AddScoped<APIIResourceFilter>();
builder.Services.AddScoped<APIIResultFilter>();
builder.Services.AddDbContext<NationalBankIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("AppSettings:IdentityDatabaseConnectionString").Value);
});
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<NationalBankIdentityDbContext>().AddDefaultTokenProviders();

builder.Services.AddDbContext<NationalBankDatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("AppSettings:DatabaseConnectionString").Value);
});
builder.Services.AddAdithyamainServices();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("MinimumAge18", policy =>
        policy.RequireClaim("Age", "18", "19", "20", "21")); // sample age claim check
});
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer =  builder.Configuration.GetSection("AppSettings:JWT:ValidIssuer").Value,
        ValidAudience = builder.Configuration.GetSection("AppSettings:JWT:ValidAudience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:JWT:Secret").Value))
    };
});
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        builder => builder
       .AllowAnyOrigin()
        .AllowAnyMethod().AllowAnyHeader());
});



var app = builder.Build();


app.UseMiddleware<ErrorHandlerMiddleware>();

//If you want to see the middleware flow then add this lines after each middleware the keep break point at await next(); 
/*app.Use(async (context, next) =>
{
    Console.WriteLine("Request: LoggingMiddleware");
    await next();
    Console.WriteLine("Response: LoggingMiddleware");
});*/

/*if (app.Environment.IsDevelopment())
{*/
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{desc.GroupName}/swagger.json",
                $"WebAPI {desc.GroupName.ToUpperInvariant()}");
        }
    });

    app.UseHttpsRedirection(); 
/*}*/
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseRateLimiter();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
//app.UseIpRateLimiting();

app.MapControllers();
app.Run();
