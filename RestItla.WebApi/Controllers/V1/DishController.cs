using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RestItla.Application.DTO.Dish;
using RestItla.Application.Extras.ResultObject;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Enum;
using RestItla.WebApi.Attributes;
using RestItla.WebApi.Extension;
namespace RestItla.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = nameof(Role.Admin))]
    public class DishController : BaseApiController
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [ValidateModel]
        [HttpPost("dish")]
        public async Task<IActionResult> Create([FromBody] DishSaveDTO dto)
        {
            await _dishService.Add(dto);
            return Created();
        }

        [ValidateModel]
        [HttpPut("dish/{id:Guid}")]
        public async Task<IActionResult> Update([FromBody] DishUpdateDTO dto, [FromRoute] Guid id)
        {
            Result<DishUpdateResponseDTO> result = await _dishService.Update(dto, id);
            return result.ToActionResult();
        }

        [HttpGet("dishes")]
        public async Task<IActionResult> List()
        {
            List<DishDTO> dishDTOs = (await _dishService.GetAll()).ToList();
            if (dishDTOs.Count == 0)
            {
                return NoContent();
            }
            return Ok(dishDTOs);
        }

        [HttpGet("dish/{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Result<DishDTO> result = await _dishService.GetById(id);
            return result.ToActionResult();
        }
    }
}