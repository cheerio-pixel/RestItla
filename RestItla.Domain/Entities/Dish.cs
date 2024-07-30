using RestItla.Domain.Common;
using RestItla.Domain.Enum;

namespace RestItla.Domain.Entities
{
    public class Dish
    : Entity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int PortionSize { get; set; }
        public DishCategory DishCategory { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}