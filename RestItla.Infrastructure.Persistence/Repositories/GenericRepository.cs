using Microsoft.EntityFrameworkCore;

using RestItla.Application.Extras;
using RestItla.Application.Interfaces.Repositories;
using RestItla.Domain.Common;
using RestItla.Infrastructure.Persistence.Context;

namespace RestItla.Infrastructure.Persistence.Repositories
{
    internal class GenericRepository<TEntity, TKey>
    : IGenericRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : struct
    {
        private readonly MainContext _context;

        public GenericRepository(MainContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<HashSet<TKey>> CheckIds(IEnumerable<TKey> keys)
        {
            HashSet<TKey> keies = keys.ToHashSet();
            HashSet<TKey> keies2 = _context
                .Set<TEntity>()
                .Select(x => x.Id)
                .Where(x => keies.Contains(x))
                .ToHashSet();
            keies.ExceptWith(keies2);
            return keies2.AsTask();
        }

        public virtual async Task Delete(TKey key)
        {
            TEntity? entity = await _context.Set<TEntity>().FindAsync(key);
            if (entity is not null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual Task<IEnumerable<TEntity>> GetAll()
        {
            return _context.Set<TEntity>().AsEnumerable().AsTask();
        }

        public virtual Task<IEnumerable<TEntity>> GetAllWith(params string[] dependencies)
        {
            return
            dependencies
            .Aggregate(
                _context.Set<TEntity>().AsQueryable(),
                (acc, it) => acc.Include(it))
            .AsEnumerable()
            .AsTask();
        }

        public virtual async Task<TEntity?> GetById(TKey key)
        {
            return await _context.Set<TEntity>().FindAsync(key);
        }

        public virtual async Task<TEntity?> GetByIdWith(TKey key, params string[] dependencies)
        {
            return await dependencies.Aggregate(
                _context.Set<TEntity>().AsQueryable(),
                (acc, it) => acc.Include(it)).FirstOrDefaultAsync(a => a.Id.Equals(key));
        }

        public async Task<bool> IdExists(TKey key)
        {
            return await _context.Set<TEntity>().AnyAsync(t => t.Id.Equals(key));
        }

        public virtual async Task<TEntity?> Update(TEntity entity)
        {
            var entry = await _context.Set<TEntity>().FindAsync(entity.Id);
            if (entry is null) return null;
            _context.Entry(entry).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }

    internal class GenericRepository<TEntity>
    : GenericRepository<TEntity, Guid>
    where TEntity : Entity
    {
        public GenericRepository(MainContext context) : base(context)
        {
        }
    }
}