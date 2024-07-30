using RestItla.Application.Extras;
using RestItla.Domain.Entities;
using RestItla.Infrastructure.Persistence.Context;

using RestItla.Domain.Interfaces.Repository;

namespace RestItla.Infrastructure.Persistence.Repositories
{
    internal class OrderRepository
    : GenericRepository<Order>,
      IOrderRepository
    {
        private readonly MainContext _context;

        public OrderRepository(MainContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<Order>> GetOrders(Guid tableId)
        {
            return _context.Orders.Where(o => o.TableId == tableId).AsEnumerable()
                                  .AsTask();
        }
    }
}