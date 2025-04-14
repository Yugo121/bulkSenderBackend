namespace Domain.Entities
{
    public class Mapping
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public List<MappingEntry> MappingEntries { get; set; }
    }
}
