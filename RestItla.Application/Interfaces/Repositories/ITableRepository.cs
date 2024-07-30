using RestItla.Domain.Entities;

namespace RestItla.Application.Interfaces.Repositories
{
    public interface ITableRepository
    : IGenericRepository<Table, Guid>
    {
    }
}