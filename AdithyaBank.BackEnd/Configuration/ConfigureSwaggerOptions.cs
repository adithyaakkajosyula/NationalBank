
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdithyaBank.BackEnd.Configuration
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, new OpenApiInfo
                {
                    Title = "WebAPI",
                    Version = desc.ApiVersion.ToString(),
                    Description = "Product WebAPI"
                });
            }

            // ✅ Your existing JWT Bearer config
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
                new List<string>()
            }
        });
        }
    }
}
