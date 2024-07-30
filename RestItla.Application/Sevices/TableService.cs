using AutoMapper;

using RestItla.Application.DTO.Order;
using RestItla.Application.DTO.Table;
using RestItla.Application.Extras.ResultObject;
using RestItla.Application.Interfaces.Repositories;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Entities;
using RestItla.Domain.Enum;

using RestItla.Domain.Interfaces.Repository;

namespace RestItla.Application.Sevices
{
    internal class TableService
    : GenericService<TableSaveDTO, TableDTO, TableUpdateDTO, TableSaveResponseDTO, TableUpdateDTO, Table, Guid>,
       ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public TableService(ITableRepository tableRepository,
                            IMapper mapper,
                            IOrderRepository orderRepository)
        : base(tableRepository, mapper)
        {
            _tableRepository = tableRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> ChangeStatus(Guid tableId, TableStatus status)
        {
            Table? table = await _tableRepository.GetById(tableId);
            if (table is null)
            {
                return ErrorType.NotFound
                                .Because("There is no table with that id");
            }

            table.Status = status;
            await _tableRepository.Update(table);
            return Unit.T;
        }

        public async Task<IEnumerable<TableOrdersDTO>> GetTableOrders(Guid tableId)
        {
            IEnumerable<Order> enumerable = await _orderRepository.GetOrders(tableId);
            enumerable = enumerable.Where(o => o.IsInProcess);
            return _mapper.Map<IEnumerable<TableOrdersDTO>>(enumerable);
        }
    }
}