using RestItla.Application.DTO.Order;
using RestItla.Application.DTO.Table;
using RestItla.Application.Extras.ResultObject;
using RestItla.Domain.Entities;
using RestItla.Domain.Enum;

namespace RestItla.Application.Interfaces.Services
{
    public interface ITableService
    : IGenericService<TableSaveDTO, TableDTO, TableUpdateDTO, TableSaveResponseDTO, TableUpdateDTO, Table, Guid>
    {
        Task<IEnumerable<TableOrdersDTO>> GetTableOrders(Guid tableId);
        Task<Result<Unit>> ChangeStatus(Guid tableId, TableStatus status);
    }
}