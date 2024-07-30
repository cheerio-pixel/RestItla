
using RestItla.Domain.Common;

namespace RestItla.Domain.Entities
{
    public class Order
    : Entity
    {
        public decimal Subtotal { get; set; }
        public bool IsInProcess { get; set; } = true;

        public Table? Table { get; set; }
        public Guid TableId { get; set; }

        public ICollection<Dish> SelectedDishes { get; set; } = new List<Dish>();
    }
}