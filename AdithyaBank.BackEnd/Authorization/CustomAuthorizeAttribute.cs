using AdithyaBank.BackEnd.Authorization;
using AdithyaBank.BackEnd.Helpers;
using AdithyaBank.BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<string> _roles;
    public string Policy { get; set; }

    public CustomAuthorizeAttribute(params string[] roles)
    {
        _roles = roles ?? new string[] { };
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Skip if AllowAnonymous
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<CustomAllowAnonymousAttribute>() != null)
        {
            return; // Allow anonymous access
        }

        var user = (UserModel)context.HttpContext.Items["User"];
        if (user == null)
        {
            throw new AppException("Unauthorized");
        }

        // Role Check
        if (_roles.Any() && !_roles.Contains(user.Role.Name))
        {
            throw new AppException("Unauthorized Role");
        }

        // Policy Check (Optional)
        if (!string.IsNullOrEmpty(Policy))
        {
            var authService = context.HttpContext.RequestServices.GetService(typeof(IAuthorizationService)) as IAuthorizationService;
            var authResult = authService?.AuthorizeAsync(context.HttpContext.User, Policy).Result;

            if (authResult == null || !authResult.Succeeded)
            {
                throw new AppException("Policy Authorization Failed");
            }
        }
    }
}
