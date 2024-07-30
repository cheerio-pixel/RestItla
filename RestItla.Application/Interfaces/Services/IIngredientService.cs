using RestItla.Application.DTO.Ingredient;
using RestItla.Domain.Entities;

namespace RestItla.Application.Interfaces.Services
{
    public interface IIngredientService
    : IGenericService<IngredientSaveDTO, IngredientSaveResponseDTO, IngredientDTO, Ingredient>
    {

    }
}