namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Ean { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string MainImage { get; set; }
        public string SecondImage { get; set; }
        public string ThirdImage { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<Property> Properties { get; set; } = new List<Property>();
    }
}
