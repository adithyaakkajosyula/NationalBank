using AdithyaBank.BackEnd.Models;
using AdithyaBank.BackEnd.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AdithyaBank.BackEnd.Helpers;
using AdithyaBank.BackEnd.Authorization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using AdithyaBank.BackEnd.Entities;
using AdithyaBank.Api.Filters;
using Serilog;
using AdithyaBank.Api.Controllers;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog(); // Add Serilog to the pipeline
builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("V1", new OpenApiInfo
    {
        Version = "V1",
        Title = "WebAPI",
        Description = "Product WebAPI"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                }
            },
            new List < string > ()
        }
    });
});

builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<APIIActionFilter>();
builder.Services.AddScoped<CustomExceptionFilter>();
builder.Services.AddDbContext<AdithyaBankIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("AppSettings:IdentityDatabaseConnectionString").Value);
});
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<AdithyaBankIdentityDbContext>().AddDefaultTokenProviders();

builder.Services.AddDbContext<AdithyaBankDatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("AppSettings:DatabaseConnectionString").Value);
});
builder.Services.AddAdithyamainServices();
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
    /*options.AddPolicy(MyAllowSpecificOrigins,
        builder => builder.AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()
        );*/
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
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "Product WebAPI");
    });
}*/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "Product WebAPI");
    });

    app.UseHttpsRedirection(); 
}

app.UseCors(MyAllowSpecificOrigins);
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
