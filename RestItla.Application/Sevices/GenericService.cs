 using AutoMapper;

using RestItla.Application.Extras.ResultObject;
using RestItla.Application.Interfaces.Repositories;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Common;
using RestItla.Domain.Enum;

namespace RestItla.Application.Sevices
{
    internal class GenericService<TSaveDTO, TViewDTO, TUpdateDTO, TSaveResponse, TUpdateResponse, TEntity, TKey>
    : IGenericService<TSaveDTO, TViewDTO, TUpdateDTO, TSaveResponse, TUpdateResponse, TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : struct
    {
        private readonly IGenericRepository<TEntity, TKey> _genericRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// An array of properties to include when pulling data
        /// </summary>
        protected virtual string[] PropertiesToInclude { get; } = Array.Empty<string>();

        public GenericService(IGenericRepository<TEntity, TKey> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public virtual async Task<TSaveResponse> Add(TSaveDTO vm)
        {
            TEntity entity = _mapper.Map<TEntity>(vm);
            return _mapper.Map<TSaveResponse>(await _genericRepository.Add(entity));
        }

        public virtual async Task Delete(TKey id)
        {
            await _genericRepository.Delete(id);
        }

        public virtual async Task<IEnumerable<TViewDTO>> GetAll()
        {
            IEnumerable<TEntity> enumerable = await _genericRepository.GetAllWith(PropertiesToInclude);
            return _mapper.Map<IEnumerable<TViewDTO>>(enumerable);
        }

        public virtual async Task<Result<TViewDTO>> GetById(TKey id)
        {
            TEntity? entity = await _genericRepository.GetByIdWith(id, PropertiesToInclude);
            if (entity is null)
            {
                return ErrorType.NotFound
                                .Because("Cannot find this resource.");
            }
            return _mapper.Map<TViewDTO>(entity);
        }

        public virtual async Task<Result<TUpdateResponse>> Update(TUpdateDTO vm, TKey id)
        {
            if (await _genericRepository.IdExists(id))
            {
                TEntity entity = _mapper.Map<TEntity>(vm);
                entity.Id = id;
                return _mapper.Map<TUpdateResponse>(await _genericRepository.Update(entity));
            }
            return ErrorType.NotFound
                            .Because("Cannot find this resource.");
        }
    }

    internal class GenericService<TSaveDTO, TSaveResponse, TViewDTO, TEntity>
    : GenericService<TSaveDTO, TViewDTO, TSaveDTO, TSaveResponse, TSaveResponse, TEntity, Guid>,
      IGenericService<TSaveDTO, TSaveResponse, TViewDTO, TEntity>
    where TEntity : Entity<Guid>
    {
        public GenericService(IGenericRepository<TEntity, Guid> genericRepository,
                              IMapper mapper)
        : base(genericRepository, mapper)
        {
        }
    }

}