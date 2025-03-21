using CsvHelper.Configuration;

namespace Infrastructure.Services
{
    public class DynamicCsvMap<T> : ClassMap<T>
    {
        public DynamicCsvMap(Dictionary<string, string> columnMappings)
        {
            foreach (var mapping in columnMappings)
            {
                var property = typeof(T).GetProperty(mapping.Value);
                if (property != null)
                {
                    Map(m => property.GetValue(m)).Name(mapping.Key);
                }
            }
        }
    }
}
