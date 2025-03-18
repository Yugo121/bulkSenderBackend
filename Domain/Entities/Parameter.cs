namespace Domain.Entities
{
    public class Parameter
    {
        public Guid Id { get; set; }
        public int BaselinkerId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
