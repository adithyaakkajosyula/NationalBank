using NationalBank.BackEnd.DataContext;
using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add MVC with runtime compilation
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// App settings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add Identity DbContext (for users/roles)
builder.Services.AddDbContext<NationalBankIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("AppSettings:IdentityDatabaseConnectionString").Value);
});

// Add Application DbContext (for other data)
builder.Services.AddDbContext<NationalBankDatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("AppSettings:DatabaseConnectionString").Value);
});

// Add Identity (User + Role) + EF + Default token providers
builder.Services
    .AddIdentity<User, Role>(options =>
    {
        // Optional: password policy, lockout, etc.
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<NationalBankIdentityDbContext>()
    .AddDefaultTokenProviders();

// Add your custom services
builder.Services.AddAdithyamainServices();

// Configure Identity Cookie (this is the one Identity uses)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/UserLogin/Login";
    options.AccessDeniedPath = "/UserLogin/AccessDenied";
    options.Cookie.Name = ".NationalBank.Identity";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
});

//  Add Newtonsoft JSON (optional)
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ContractResolver = new DefaultContractResolver());


var app = builder.Build();

//  Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// These must come in this exact order
app.UseAuthentication();
app.UseAuthorization();

// Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
