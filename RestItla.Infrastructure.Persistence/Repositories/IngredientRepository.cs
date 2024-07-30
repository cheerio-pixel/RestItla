using RestItla.Application.Interfaces.Repositories;
using RestItla.Domain.Entities;
using RestItla.Infrastructure.Persistence.Context;

namespace RestItla.Infrastructure.Persistence.Repositories
{
    internal class IngredientRepository
    : GenericRepository<Ingredient>,
      IIngredientRepository
    {
        private readonly MainContext _context;

        public IngredientRepository(MainContext context) : base(context)
        {
            _context = context;
        }
    }
}