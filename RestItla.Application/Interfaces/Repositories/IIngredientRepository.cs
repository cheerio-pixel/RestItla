using RestItla.Domain.Entities;

namespace RestItla.Application.Interfaces.Repositories
{
    public interface IIngredientRepository
    : IGenericRepository<Ingredient, Guid>
    {
    }
}