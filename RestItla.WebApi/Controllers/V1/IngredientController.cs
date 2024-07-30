using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RestItla.Application.DTO.Ingredient;
using RestItla.Application.Extras.ResultObject;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Enum;
using RestItla.WebApi.Attributes;
using RestItla.WebApi.Extension;
namespace RestItla.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = nameof(Role.Admin))]
    public class IngredientController : BaseApiController
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [ValidateModel]
        [HttpPost("ingredient")]
        public async Task<IActionResult> Create([FromBody] IngredientSaveDTO dto)
        {
            await _ingredientService.Add(dto);
            return Created();
        }

        [ValidateModel]
        [HttpPut("ingredient/{id:Guid}")]
        public async Task<IActionResult> Update([FromBody] IngredientSaveDTO dto, [FromRoute] Guid id)
        {
            Result<IngredientSaveResponseDTO> result = await _ingredientService.Update(dto, id);
            return result.ToActionResult();
        }

        [HttpGet("ingredients")]
        public async Task<IActionResult> List()
        {
            List<IngredientDTO> ingredientDTOs = (await _ingredientService.GetAll()).ToList();
            if (ingredientDTOs.Count == 0)
            {
                return NoContent();
            }
            return Ok(ingredientDTOs);
        }

        [HttpGet("ingredient/{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Result<IngredientDTO> result = await _ingredientService.GetById(id);
            return result.ToActionResult();
        }
    }
}