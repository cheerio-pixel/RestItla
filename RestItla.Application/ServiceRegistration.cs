using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using RestItla.Application.Interfaces.Services;
using RestItla.Application.Sevices;

namespace RestItla.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            return
            services.AddAutoMapper(Assembly.GetExecutingAssembly())
                    .AddTransient(typeof(IGenericService<,,,,,,>), typeof(GenericService<,,,,,,>))
                    .AddTransient<IDishService, DishService>()
                    .AddTransient<IOrderService, OrderService>()
                    .AddTransient<IIngredientService, IngredientService>()
                    .AddTransient<ITableService, TableService>();
        }
    }
}