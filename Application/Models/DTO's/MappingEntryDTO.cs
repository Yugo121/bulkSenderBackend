using System.Text.Json.Serialization;
using Domain.Enums;

namespace Application.Models.DTO_s
{
    public class MappingEntryDTO
    {
        public Guid Id { get; set; }
        public string ColumnName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MappingType MappingType { get; set; }
        public string TargetField { get; set; }
        public MappingEntryDTO() { }
    }
}
