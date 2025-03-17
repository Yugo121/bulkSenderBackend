namespace Domain.Entities
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
