using RestItla.Application.Extras.ResultObject;
using RestItla.Domain.Common;

namespace RestItla.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : struct
    {
        public Task<TEntity> Add(TEntity entity);
        public Task<TEntity?> Update(TEntity entity);
        public Task Delete(TKey key);
        public Task<IEnumerable<TEntity>> GetAll();
        public Task<TEntity?> GetById(TKey key);
        public Task<IEnumerable<TEntity>> GetAllWith(params string[] dependencies);
        public Task<TEntity?> GetByIdWith(TKey key, params string[] dependencies);

        public Task<bool> IdExists(TKey key);
        /// <summary>
        /// Return the set of keys that do not exist.
        /// </summary>
        public Task<HashSet<TKey>> CheckIds(IEnumerable<TKey> keys);
    }
}