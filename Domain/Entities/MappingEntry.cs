using System.Data;

namespace Domain.Entities
{
    public class MappingEntry
    {
        public Guid Id { get; set; }
        public string ColumnName { get; set; }
        public string TargetField { get; set; }
        public MappingType MappingType { get; set; }
        public Guid MappingId { get; set; }
        public Mapping Mapping { get; set; }
    }
}
