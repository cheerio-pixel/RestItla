using System.Text.Json.Serialization;

using RestItla.Domain.Enum;

namespace RestItla.Application.DTO.Table
{
    public class TableDTO
    {
        public Guid Id { get; set; }
        public int PeopleQuantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public TableStatus Status { get; set; }
    }
}