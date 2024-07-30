using RestItla.Application.DTO.Order;
using RestItla.Domain.Entities;

namespace RestItla.Application.Interfaces.Services
{
    public interface IOrderService
    : IGenericService<OrderSaveDTO, OrderDTO, OrderUpdateDTO, OrderSaveResponseDTO, OrderUpdateResponseDTO, Order, Guid>
    {
    }
}