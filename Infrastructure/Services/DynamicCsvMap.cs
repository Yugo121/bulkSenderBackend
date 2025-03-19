using CsvHelper.Configuration;

namespace Infrastructure.Services
{
    public class DynamicCsvMap<T> : ClassMap<T>
    {
        public DynamicCsvMap(Dictionary<string, string> columnMappings)
        {
            foreach (var mapping in columnMappings)
            {
                //Map(typeof(T).GetProperty(mapping.Value)).Name(mapping.Key);
            }
        }
    }
}
