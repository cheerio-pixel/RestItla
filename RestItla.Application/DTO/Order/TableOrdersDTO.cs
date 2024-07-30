using RestItla.Application.DTO.Dish;

namespace RestItla.Application.DTO.Order
{
    public class TableOrdersDTO
    {
        public Guid Id { get; set; }
        public decimal Subtotal { get; set; }
        public bool IsInProcess { get; set; }

        public ICollection<DishDTO> SelectedDishes { get; set; } = new List<DishDTO>();
    }
}