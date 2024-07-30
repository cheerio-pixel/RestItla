using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using RestItla.Domain.Enum;

namespace RestItla.Application.DTO.Table
{
    public class TableSaveDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PeopleQuantity { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;

        [JsonIgnore]
        public TableStatus Status { get; set; } = TableStatus.Avalible;
    }
}