using AutoMapper;

using RestItla.Application.DTO.Ingredient;
using RestItla.Application.Interfaces.Repositories;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Entities;


namespace RestItla.Application.Sevices
{
    internal class IngredientService
    : GenericService<IngredientSaveDTO, IngredientSaveResponseDTO, IngredientDTO, Ingredient>,
      IIngredientService
    {
        public IngredientService(IIngredientRepository ingredientRepository, IMapper mapper)
        : base(ingredientRepository, mapper)
        {
        }
    }

}