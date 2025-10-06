using NationalBank.BackEnd.DataContext;
using NationalBank.BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NationalBank.BackEnd.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using NationalBank.BackEnd.Authorization;
using Microsoft.AspNetCore.Builder;
//https://code-maze.com/identity-asp-net-core-project/
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

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
//This token services is only for Identity Authentication token etc
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/UserLogin/Login";
    options.Cookie.Name = ".AspNetCore.Identity.Application";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/UserLogin/AccessDenied";
});
builder.Services
.AddControllersWithViews()
.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());


//This is for our custom token by JSON web tokens for api services 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "LoginUser";
            });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// its for redirecting http to https 
app.UseHttpsRedirection();
// this service is used to register services for static files 
app.UseStaticFiles();

app.UseRouting();
// it will authenticate user that is obtained by request to enter into sepecific controller
app.UseAuthentication();
// if user is authenticated then user details is appended to http context Then we can use the user information 
// for our requirement Like signed by xuser
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
