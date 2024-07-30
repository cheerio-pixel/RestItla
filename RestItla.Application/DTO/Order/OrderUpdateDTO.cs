using System.ComponentModel.DataAnnotations;

namespace RestItla.Application.DTO.Order
{
    public class OrderUpdateDTO
    {
        [Required]
        public ICollection<Guid> DishesToRemove { get; set; } = new List<Guid>();

        [Required]
        public ICollection<Guid> DishesToAdd { get; set; } = new List<Guid>();
    }
}