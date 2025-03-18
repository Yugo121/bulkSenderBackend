using Domain.Entities;

namespace Application.Models.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public int BaselinkerId { get; set; }
        public string Sku { get; set; }
        public string Ean { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string MainImage { get; set; }
        public string SecondImage { get; set; }
        public string ThirdImage { get; set; }
        public CategoryDTO Category { get; set; }
        public BrandDTO Brand { get; set; }
        public List<ParameterDTO> Parameters { get; set; } = new List<ParameterDTO>();

        ProductDTO(Product product)
        {
            Id = product.Id;
            Sku = product.Sku;
            Ean = product.Ean;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            MainImage = product.MainImage;
            SecondImage = product.SecondImage;
            ThirdImage = product.ThirdImage;
            Category = new CategoryDTO(product.Category);
            Brand = new BrandDTO(product.Brand);
            Parameters = product.Parameters.Select(p => new ParameterDTO(p)).ToList();
        }
    }
}
