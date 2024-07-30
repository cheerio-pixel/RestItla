using Microsoft.EntityFrameworkCore;

using RestItla.Application.Interfaces.Repositories;
using RestItla.Domain.Entities;
using RestItla.Infrastructure.Persistence.Context;

namespace RestItla.Infrastructure.Persistence.Repositories
{
    internal class DishIngredientRepository
    : RelationShipRepository<DishIngredient>, IDishIngredientRepository
    {
        private readonly MainContext _context;

        public DishIngredientRepository(MainContext context) : base(context)
        {
            _context = context;
        }
    }
}