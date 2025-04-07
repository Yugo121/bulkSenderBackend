namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public int BaselinkerId { get; set; }
        public string Sku { get; set; }
        public string Ean { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
    }
}
