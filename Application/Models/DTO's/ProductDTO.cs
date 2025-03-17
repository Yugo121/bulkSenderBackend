using Domain.Entities;

namespace Application.Models.DTOs
{
    public class ProductDTO
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
        public CategoryDTO Category { get; set; }
        public BrandDTO Brand { get; set; }
        public List<PropertyDTO> Properties { get; set; } = new List<PropertyDTO>();

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
            Properties = product.Properties.Select(p => new PropertyDTO(p)).ToList();
        }
    }
}
