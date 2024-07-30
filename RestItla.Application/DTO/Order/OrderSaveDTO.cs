using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestItla.Application.DTO.Order
{
    public class OrderSaveDTO
    {
        [Required]
        [JsonIgnore]
        public decimal Subtotal { get; set; }
        [Required]
        public bool IsInProcess { get; set; } = true;

        [Required]
        public Guid TableId { get; set; }

        [Required]
        public ICollection<Guid> SelectedDishes { get; set; } = new List<Guid>();
    }
}