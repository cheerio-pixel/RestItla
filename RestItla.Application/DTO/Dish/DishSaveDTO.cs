using System.ComponentModel.DataAnnotations;

using RestItla.Domain.Enum;

namespace RestItla.Application.DTO.Dish
{
    public class DishSaveDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(typeof(decimal), "1", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        [Required]
        public int PortionSize { get; set; }
        [Required]
        public DishCategory DishCategory { get; set; }

        [Required]
        public ICollection<Guid> Ingredients { get; set; } = new List<Guid>();
    }
}