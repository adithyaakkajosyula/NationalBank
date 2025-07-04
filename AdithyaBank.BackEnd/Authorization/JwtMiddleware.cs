namespace NationalBank.BackEnd.Authorization;

using NationalBank.BackEnd.Models;
using NationalBank.BackEnd.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserRepository userService, IJwtUtils jwtUtils)
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        // Check if the header is present and starts with 'Bearer '
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var parts = authorizationHeader.Split(" ");
            if (parts.Length == 2 && !string.IsNullOrEmpty(parts[1]))
            {
                var token = parts[1];

                // Validate JWT and get result
                var apiBaseResultModel = jwtUtils.ValidateJwtToken(token);
                if (apiBaseResultModel.IsSuccess && apiBaseResultModel.Data is JwtSecurityToken jwtToken)
                {
                    //  Set ClaimsPrincipal (context.User) from token's claims
                    var claims = jwtToken.Claims.ToList();
                    var identity = new ClaimsIdentity(claims, "CustomJwt");
                    var principal = new ClaimsPrincipal(identity);
                    context.User = principal;

                    //  Optional: Attach the full user object to HttpContext.Items
                    context.Items["User"] = await userService.GetById(apiBaseResultModel.Id);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(apiBaseResultModel.Message);
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid Authorization header format.");
                return;
            }
        }

        await _next(context); // Proceed to next middleware
    }
}