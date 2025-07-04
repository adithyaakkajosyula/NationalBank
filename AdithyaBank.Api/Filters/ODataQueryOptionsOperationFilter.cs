
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ODataQueryOptionsOperationFilter : IOperationFilter
{
    private static readonly string[] Options =
        { "$select", "$filter", "$orderby", "$expand", "$top", "$skip", "$count" };

    public void Apply(OpenApiOperation op, OperationFilterContext ctx)
    {
        if (!ctx.MethodInfo.GetCustomAttributes(true)
                           .Any(a => a.GetType().Name == "EnableQueryAttribute"))
            return;

        foreach (var name in Options)
            op.Parameters.Add(new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema { Type = "string" },
                Description = "OData query option",
                Required = false
            });
    }
}
