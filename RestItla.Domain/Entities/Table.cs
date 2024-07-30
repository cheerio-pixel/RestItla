
using RestItla.Domain.Common;
using RestItla.Domain.Enum;

namespace RestItla.Domain.Entities
{
    public class Table
    : Entity
    {
        public int PeopleQuantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public TableStatus Status { get; set; }
    }
}