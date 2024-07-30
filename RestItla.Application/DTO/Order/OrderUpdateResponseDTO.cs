using RestItla.Application.DTO.Dish;
using RestItla.Application.DTO.Table;

namespace RestItla.Application.DTO.Order
{
    public class OrderUpdateResponseDTO
    {
        public Guid Id { get; set; }
        public decimal Subtotal { get; set; }
        public bool IsInProcess { get; set; }

        public TableDTO? Table { get; set; }

        public ICollection<DishDTO> SelectedDishes { get; set; } = new List<DishDTO>();

    }
}