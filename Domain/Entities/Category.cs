namespace Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string BaselinkerName { get; set; }
        public int BaselinkerId { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<CategoryAlias> Aliases { get; set; }
    }
}
