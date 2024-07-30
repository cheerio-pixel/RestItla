using RestItla.Application.Interfaces.Repositories;
using RestItla.Domain.Entities;
using RestItla.Infrastructure.Persistence.Context;

namespace RestItla.Infrastructure.Persistence.Repositories
{
    internal class TableRepository
    : GenericRepository<Table>,
      ITableRepository
    {
        private readonly MainContext _context;

        public TableRepository(MainContext context) : base(context)
        {
            _context = context;
        }
    }
}