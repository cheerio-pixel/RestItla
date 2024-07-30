using AutoMapper;

using RestItla.Application.DTO.Order;
using RestItla.Application.Extras.ResultObject;
using RestItla.Application.Interfaces.Repositories;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Entities;
using RestItla.Domain.Enum;

using RestItla.Domain.Interfaces.Repository;

namespace RestItla.Application.Sevices
{
    internal class OrderService
    : GenericService<OrderSaveDTO, OrderDTO, OrderUpdateDTO, OrderSaveResponseDTO, OrderUpdateResponseDTO, Order, Guid>,
         IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IMapper _mapper;
        private readonly IDishOrderRepository _dishOrderRepository;

        protected override string[] PropertiesToInclude
        => new string[] { "SelectedDishes", "SelectedDishes.Ingredients", "Table" };

        public OrderService(IOrderRepository orderRepository,
                            IMapper mapper,
                            IDishRepository dishRepository,
                            IDishOrderRepository dishOrderRepository)
        : base(orderRepository, mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _dishRepository = dishRepository;
            _dishOrderRepository = dishOrderRepository;
        }

        public override async Task<OrderSaveResponseDTO> Add(OrderSaveDTO vm)
        {
            OrderSaveResponseDTO orderSaveResponseDTO = await base.Add(vm);
            IEnumerable<DishOrder> dishOrders = vm.SelectedDishes.Select(uid => new DishOrder()
            {
                DishId = uid,
                OrderId = orderSaveResponseDTO.Id
            });

            await _dishOrderRepository.Create(dishOrders.ToArray());

            Order? order = await _orderRepository.GetByIdWith(orderSaveResponseDTO.Id, PropertiesToInclude);
            order!.Subtotal = order.SelectedDishes.Select(a => a.Price).Aggregate(0m, (acc, it) => acc + it);
            Order? order1 = await _orderRepository.Update(order);

            return _mapper.Map<OrderSaveResponseDTO>(order1);
        }

        public override async Task<Result<OrderUpdateResponseDTO>> Update(OrderUpdateDTO vm, Guid id)
        {
            if (await _orderRepository.IdExists(id))
            {
                HashSet<Guid> guids = await _dishRepository.CheckIds(vm.DishesToAdd);
                if (guids.Any())
                {
                    return ErrorType.NotFound
                                    .Because($"{string.Join(", ", guids)} do not exist");
                }
                var dishOrdersToRemove = vm.DishesToRemove.Select(ToDishOrder).ToArray();
                var dishOrdersToAdd = vm.DishesToAdd.Select(ToDishOrder).ToArray();

                await _dishOrderRepository.Create(dishOrdersToAdd);
                await _dishOrderRepository.Remove(dishOrdersToRemove);

                Order? order = await _orderRepository.GetByIdWith(id, PropertiesToInclude);
                if (order is null)
                {
                    return ErrorType.NotFound
                                    .Because("Cannot find this resource.");
                }

                order.Subtotal = order.SelectedDishes.Select(a => a.Price).Aggregate(0m, (acc, it) => acc + it);
                await _orderRepository.Update(order);

                return _mapper.Map<OrderUpdateResponseDTO>(
                    order
                );
            }

            return ErrorType.NotFound
                            .Because("Cannot find this resource.");

            DishOrder ToDishOrder(Guid d)
            {
                return new DishOrder()
                {
                    DishId = d,
                    OrderId = id
                };
            }
        }
    }
}