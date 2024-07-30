using RestItla.Application.Interfaces.Repositories;
using RestItla.Domain.Entities;
using RestItla.Infrastructure.Persistence.Context;

namespace RestItla.Infrastructure.Persistence.Repositories
{
    internal class DishRepository
    : GenericRepository<Dish>,
      IDishRepository
    {
        private readonly MainContext _context;

        public DishRepository(MainContext context) : base(context)
        {
            _context = context;
        }
    }
}