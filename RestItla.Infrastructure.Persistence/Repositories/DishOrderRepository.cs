using RestItla.Application.Interfaces.Repositories;
using RestItla.Domain.Entities;
using RestItla.Infrastructure.Persistence.Context;

namespace RestItla.Infrastructure.Persistence.Repositories
{
    internal class DishOrderRepository
    : RelationShipRepository<DishOrder>, IDishOrderRepository
    {
        public DishOrderRepository(MainContext context) : base(context)
        {
        }
    }
}