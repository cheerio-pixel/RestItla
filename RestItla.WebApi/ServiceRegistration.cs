using Asp.Versioning;

using Microsoft.OpenApi.Models;

using RestItla.WebApi.Middleware;

namespace RestItla.WebApi
{
    internal static class ServiceRegistration
    {
        public static IServiceCollection AddApiLayer(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
             {
                 swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                 {
                     Name = "Authorization",
                     Type = SecuritySchemeType.ApiKey,
                     Scheme = "Bearer",
                     BearerFormat = "JWT",
                     In = ParameterLocation.Header,
                     Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                 });
                 swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                             {
                                   new OpenApiSecurityScheme
                                     {
                                         Reference = new OpenApiReference
                                         {
                                             Type = ReferenceType.SecurityScheme,
                                             Id = "Bearer"
                                         }
                                     },
                                     new string[] {}
                             }
                 });
                 swagger.SwaggerDoc("v1", new OpenApiInfo
                 {
                     Title = "Restaurant ITLA API",
                     Version = "v1"
                 });
             });

            // https://stackoverflow.com/questions/76371992/which-package-should-be-used-for-versioning-api-controllers-in-net-7-microsoft
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            return services.AddScoped<ApiErrorExceptionMiddleware>();
        }

        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiErrorExceptionMiddleware>();
        }

    }
}