using System.ComponentModel.DataAnnotations;

namespace RestItla.Application.DTO.Table
{
    public class TableUpdateDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PeopleQuantity { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}