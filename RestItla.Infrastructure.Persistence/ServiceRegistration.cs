using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RestItla.Application.Interfaces.Repositories;
using RestItla.Infrastructure.Persistence.Context;
using RestItla.Infrastructure.Persistence.Repositories;

using RestItla.Domain.Interfaces.Repository;

namespace RestItla.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersisentenceLayer(this IServiceCollection services, IConfiguration config)
        {
            string? connectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("DefaultConnection cannot be unset in the configuration.");
            }

            services.AddDbContext<MainContext>(options =>
                options.UseSqlServer(connectionString)
            );
            return
            services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>))
                    .AddTransient(typeof(IRelationShipRepository<>), typeof(RelationShipRepository<>))
                    .AddTransient<IDishIngredientRepository, DishIngredientRepository>()
                    .AddTransient<IDishOrderRepository, DishOrderRepository>()
                    .AddTransient<IDishRepository, DishRepository>()
                    .AddTransient<IIngredientRepository, IngredientRepository>()
                    .AddTransient<IOrderRepository, OrderRepository>()
                    .AddTransient<ITableRepository, TableRepository>();
        }
    }
}