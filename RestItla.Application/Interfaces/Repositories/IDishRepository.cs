using RestItla.Domain.Entities;

namespace RestItla.Application.Interfaces.Repositories
{
    public interface IDishRepository
    : IGenericRepository<Dish, Guid>
    {
    }
}