using RestItla.Application.Extras.ResultObject;
using RestItla.Domain.Common;

namespace RestItla.Application.Interfaces.Services
{
    public interface IGenericService<TSaveDTO, TViewDTO, TUpdateDTO, TSaveResponse, TUpdateResponse, TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : struct
    {
        Task<Result<TUpdateResponse>> Update(TUpdateDTO vm, TKey id);

        Task<TSaveResponse> Add(TSaveDTO vm);

        Task Delete(TKey id);

        Task<Result<TViewDTO>> GetById(TKey id);

        Task<IEnumerable<TViewDTO>> GetAll();
    }

    public interface IGenericService<TSaveDTO, TSaveResponse, TViewDTO, TEntity>
    : IGenericService<TSaveDTO, TViewDTO, TSaveDTO, TSaveResponse, TSaveResponse, TEntity, Guid>
    where TEntity : Entity<Guid>
    {
    }
}