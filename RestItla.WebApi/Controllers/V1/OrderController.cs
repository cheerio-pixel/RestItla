using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RestItla.Application.DTO.Order;
using RestItla.Application.Extras.ResultObject;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Enum;
using RestItla.WebApi.Attributes;
using RestItla.WebApi.Extension;

namespace RestItla.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = nameof(Role.Waiter))]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [ValidateModel]
        [HttpPost("order")]
        public async Task<IActionResult> Create([FromBody] OrderSaveDTO dto)
        {
            await _orderService.Add(dto);
            return Created();
        }

        [ValidateModel]
        [HttpPut("order/{id:Guid}")]
        public async Task<IActionResult> Update([FromBody] OrderUpdateDTO dto, [FromRoute] Guid id)
        {
            Result<OrderUpdateResponseDTO> result = await _orderService.Update(dto, id);
            return result.ToActionResult();
        }

        [HttpGet("orders")]
        public async Task<IActionResult> List()
        {
            List<OrderDTO> orderDTOs = (await _orderService.GetAll()).ToList();
            if (orderDTOs.Count == 0)
            {
                return NoContent();
            }
            return Ok(orderDTOs);
        }

        [HttpGet("order/{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Result<OrderDTO> result = await _orderService.GetById(id);
            return result.ToActionResult();
        }

        [HttpDelete("order/{id:Guid}")]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            await _orderService.Delete(id);
            return NoContent();
        }
    }
}