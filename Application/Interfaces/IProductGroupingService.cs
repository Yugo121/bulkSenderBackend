using Application.Models.DTO_s;
using Application.Models.DTOs;

namespace Application.Interfaces
{
    public interface IProductGroupingService
    {
        public Task<List<ProductDTO>> GroupProductsAsync(ProductDTO product, CancellationToken cancellationToken);
        public Task UpdateParentIdAsync(List<ProductDTO> children, int parentId, CancellationToken cancellationToken);
        public Task<MappingDTO> GetMappingForProduct(ProductDTO product, CancellationToken cancellationToken);
        public Task<bool> SetProductBaselinkerFlagAndId(ProductDTO product, int id, CancellationToken cancellationToken);
    }
}
