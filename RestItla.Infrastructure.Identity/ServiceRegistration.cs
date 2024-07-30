using System.Net;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using RestItla.Application.Extras.ResultObject;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Enum;
using RestItla.Domain.Settings;
using RestItla.Infrastructure.Identity.Context;
using RestItla.Infrastructure.Identity.Entities;
using RestItla.Infrastructure.Identity.Services;

namespace RestItla.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddIdentityLayer(this IServiceCollection services,
                                                          IConfiguration config)
        {
            string? connectionString = config.GetConnectionString("IdentityConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("IdentityConnection cannot be unset in the config.");
            }

            services.AddDbContext<RestItlaIdentityContext>(options =>
                {
                    options.UseSqlServer(connectionString,
                    m => m.MigrationsAssembly(typeof(RestItlaIdentityContext).Assembly.FullName));
                });


            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<RestItlaIdentityContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            services.Configure<JwtSettings>(config.GetSection("Jwt"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? ""))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        c.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(
                            ErrorType.Unauthorized.Because("You are not Authorized")
                        );
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        c.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(ErrorType.Unauthorized.Because("You are not Authorized to access this resource"));
                        return c.Response.WriteAsync(result);
                    }
                };
            });

            return services.AddTransient<IAccountService, AccountService>();
        }
    }
}