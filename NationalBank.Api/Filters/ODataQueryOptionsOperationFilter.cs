using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace NationalBank.BackEnd.Configuration;

/// <summary>
/// • Strips ?api-version from every operation.
/// • Adds OData query options to actions that have [EnableQuery].
/// </summary>
public sealed class ODataQueryOptionsOperationFilter : IOperationFilter
{
    private static readonly string[] ODataOptions =
    {
        "$select", "$filter", "$orderby", "$expand", "$top", "$skip", "$count"
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext ctx)
    {
        // ────────────────────────────────────────────────────────────────
        // 1️⃣  Remove api‑version query parameter
        // ────────────────────────────────────────────────────────────────
        operation.Parameters = operation.Parameters
                 .Where(p => !(p.In == ParameterLocation.Query && p.Name == "api-version"))
                 .ToList();

        // ────────────────────────────────────────────────────────────────
        // 2️⃣  Add OData parameters only if [EnableQuery] is present
        // ────────────────────────────────────────────────────────────────
        bool isEnableQuery =
            ctx.MethodInfo.GetCustomAttribute<EnableQueryAttribute>() != null ||
            ctx.MethodInfo.DeclaringType?.GetCustomAttribute<EnableQueryAttribute>() != null;

        if (!isEnableQuery) return;

        // Avoid duplicating parameters if multiple filters run
        foreach (var name in ODataOptions)
        {
            if (operation.Parameters.Any(p => p.Name == name)) continue;

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Query,
                Description = "OData query option",
                Required = false,
                Schema = new OpenApiSchema { Type = "string" },
                Example = new OpenApiString(name switch
                {
                    "$top" => "10",
                    "$skip" => "20",
                    "$count" => "true",
                    _ => ""
                })
            });
        }
    }
}
