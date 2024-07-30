using System.ComponentModel.DataAnnotations;

namespace RestItla.Application.DTO.Ingredient
{
    public class IngredientSaveDTO
    {
        [Required]
        public required string Name { get; set; }
    }
}