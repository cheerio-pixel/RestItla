using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RestItla.Application.DTO.Order;
using RestItla.Application.DTO.Table;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Enum;
using RestItla.WebApi.Attributes;
using RestItla.WebApi.Extension;
namespace RestItla.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class TableController : BaseApiController
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        // I don't think we should report 500 since the client cannot
        // handle it, since it is not its fault
        [ValidateModel]
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost("table")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] TableSaveDTO dto)
        {
            await _tableService.Add(dto);
            return Created();
        }

        [ValidateModel]
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPut("table/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableUpdateDTO))]
        public async Task<IActionResult> Update([FromBody] TableUpdateDTO dto, [FromRoute] Guid id)
        {
            return
            (await _tableService.Update(dto, id))
                                .ToActionResult();
        }

        [Authorize]
        [HttpGet("tables")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TableSaveDTO>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> List()
        {
            List<TableDTO> tableDTOs = (await _tableService.GetAll()).ToList();
            if (tableDTOs.Count == 0)
            {
                return NoContent();
            }
            return Ok(tableDTOs);
        }

        [Authorize]
        [HttpGet("table/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return (await _tableService.GetById(id))
                                       .ToActionResult();
        }

        [Authorize(Roles = nameof(Role.Waiter))]
        [HttpGet("table/{id:Guid}/orders")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableOrdersDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetTableOrders([FromRoute] Guid id)
        {
            List<TableOrdersDTO> tableOrdersDTOs = (await _tableService.GetTableOrders(id)).ToList();
            return tableOrdersDTOs.Count == 0 ?
                NoContent() :
                Ok(tableOrdersDTOs);
        }

        [Authorize(Roles = nameof(Role.Waiter))]
        [HttpPatch("table/{id:Guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, ChangeStatusDTO dto)
        {
            await _tableService.ChangeStatus(id, dto.Status);
            return NoContent();
        }
    }
}