using System.ComponentModel.DataAnnotations;

namespace RestItla.Application.DTO.Ingredient
{
    public class IngredientDTO
    {
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}