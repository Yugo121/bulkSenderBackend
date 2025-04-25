using Application.Models.DTO_s;
using Application.Models.DTOs;

namespace Application.Interfaces
{
    public interface IProductPreparationService
    {
        public string GenerateTitle(ProductDTO product, MappingDTO mapping);
        public string GenerateDescription(ProductDTO product, MappingDTO mapping);
        public string GenerateParametersString(ProductDTO product);
        public ProductToBaselinkerDTO PrepareProduct(ProductDTO product, MappingDTO mapping);
        public string ExtractMainSku(string sku);
    }
}
