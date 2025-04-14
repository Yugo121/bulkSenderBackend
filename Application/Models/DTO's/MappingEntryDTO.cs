using System.Data;
using System.Text.Json.Serialization;

namespace Application.Models.DTO_s
{
    public class MappingEntryDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string ColumnName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MappingType MappingType { get; set; }
        public string TargetField { get; set; }
        public MappingEntryDTO() { }
    }
}
