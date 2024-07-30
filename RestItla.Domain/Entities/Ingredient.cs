
using RestItla.Domain.Common;

namespace RestItla.Domain.Entities
{
    public class Ingredient
    : Entity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }
}