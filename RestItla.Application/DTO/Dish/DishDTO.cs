using System.Text.Json.Serialization;

using RestItla.Application.DTO.Ingredient;
using RestItla.Domain.Enum;

namespace RestItla.Application.DTO.Dish
{
    public class DishDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PortionSize { get; set; }
        public DishCategory DishCategory { get; set; }

        public ICollection<IngredientDTO> Ingredients { get; set; } = new List<IngredientDTO>();
    }
}