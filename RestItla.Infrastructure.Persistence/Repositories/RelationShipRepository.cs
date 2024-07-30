using Microsoft.EntityFrameworkCore;

using RestItla.Application.Interfaces.Repositories;
using RestItla.Infrastructure.Persistence.Context;

namespace RestItla.Infrastructure.Persistence.Repositories
{
    internal class RelationShipRepository<TRelation>
    : IRelationShipRepository<TRelation>
    where TRelation : class
    {
        private readonly MainContext _context;

        public RelationShipRepository(MainContext context)
        {
            _context = context;
        }

        public async Task<ICollection<TRelation>> Create(TRelation[] relations)
        {
            _context.Set<TRelation>().AddRange(relations);
            await _context.SaveChangesAsync();
            return relations;
        }

        public async Task Remove(TRelation[] relations)
        {
            _context.Set<TRelation>().RemoveRange(relations);
            await _context.SaveChangesAsync();
        }
    }
}