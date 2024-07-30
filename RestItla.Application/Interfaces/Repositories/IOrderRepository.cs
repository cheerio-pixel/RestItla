
using RestItla.Application.Interfaces.Repositories;
using RestItla.Domain.Entities;

namespace RestItla.Domain.Interfaces.Repository
{
    public interface IOrderRepository
    : IGenericRepository<Order, Guid>
    {
        Task<IEnumerable<Order>> GetOrders(Guid tableId);
    }
}