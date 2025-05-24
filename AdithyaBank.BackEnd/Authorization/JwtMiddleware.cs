namespace AdithyaBank.BackEnd.Authorization;

using AdithyaBank.BackEnd.Models;
using AdithyaBank.BackEnd.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

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

        // Check if the header is present and starts with 'Bearer'
        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            // Ensure there is exactly one 'Bearer' and a valid token part after it
            var parts = authorizationHeader.Split(" ");
            if (parts.Length == 2 && !string.IsNullOrEmpty(parts[1]))
            {
                var token = parts[1]; // Extract the token part
                var apiBaseResultModel = jwtUtils.ValidateJwtToken(token);
                if (apiBaseResultModel.IsSuccess == true)
                {
                    // Attach user to context on successful JWT validation
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
                // If there's more than one 'Bearer', consider it invalid
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid Authorization header format.");
                return;
            }
        }

        await _next(context); // Proceed to the next middleware
    }
}