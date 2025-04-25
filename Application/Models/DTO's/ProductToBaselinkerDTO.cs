namespace Application.Models.DTO_s
{
    public class ProductToBaselinkerDTO
    {
        public int ParentId { get; set; }
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Ean { get; set; }
        public Dictionary<int, decimal> Prices { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public Dictionary<string, string> TextFields { get; set; }
    }
}
