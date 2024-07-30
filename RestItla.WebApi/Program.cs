
using System.Text.Json.Serialization;

using AutoMapper;

using Microsoft.AspNetCore.Identity;

using RestItla.Application;
using RestItla.Infrastructure.Identity;
using RestItla.Infrastructure.Identity.Entities;
using RestItla.Infrastructure.Identity.Seeds;
using RestItla.Infrastructure.Persistence;
using RestItla.WebApi;

namespace RestItla.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddApplicationLayer()
                        .AddPersisentenceLayer(builder.Configuration)
                        .AddIdentityLayer(builder.Configuration)
                        .AddApiLayer();

        builder.Services.AddControllers()
                        .AddJsonOptions(options =>
                         options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                IMapper mapper = services.GetRequiredService<IMapper>();
                mapper.ConfigurationProvider.AssertConfigurationIsValid();
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await DefaultRoles.SeedAsync(userManager, roleManager);
                await DefaultUsers.SeedAsync(userManager, roleManager);
            }
#pragma warning disable RCS1075, CS0168
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
#pragma warning restore RCS1075, CS0168
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCustomExceptionMiddleware();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
