using API.Entities.Constants;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Core.Extensions
{
    public static class SwaggerExtension
    {
        /// <summary>
        /// This methods registers Swagger
        /// </summary>
        /// <param name="title">Title to be shown in Swagger</param>
        /// <returns>An instance of IServiceCollection</returns>
        public static IServiceCollection AddAutomaticApiDocumentation(this IServiceCollection services, string title)
        {
            services.AddSwaggerGen(opt => 
            {
                var apiVersionDescriptionProvider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    opt.SwaggerDoc(description.GroupName,
                        new OpenApiInfo
                        {
                            Title = title,
                            Version = description.ApiVersion.ToString(),
                            Description = string.Concat(title, " ", description.ApiVersion.ToString())
                        });
                }
                opt.AddSecurityDefinition(Constants.JWT, 
                    new OpenApiSecurityScheme() 
                    { 
                        Description = Constants.BearerDescription,
                        Name = Constants.Authorization,
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = Constants.Bearer
                    });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement() 
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = Constants.JWT
                            }
                        },
                        System.Array.Empty<string>()
                    }                   
                });
            });
            return services;
        }
    }
}