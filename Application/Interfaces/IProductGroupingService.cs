using Application.Models.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductGroupingService
    {
        public Task<(string mainSku, List<ProductDTO> children)> GroupProductsAsync(ProductDTO product, CancellationToken cancellationToken);
        public Task UpdateParentIdAsync(List<ProductDTO> children, int parentId, CancellationToken cancellationToken);
    }
}
