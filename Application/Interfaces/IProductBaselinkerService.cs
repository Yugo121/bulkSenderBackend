using Application.Models.DTO_s;
using Application.Models.DTOs;

namespace Application.Interfaces
{
    public interface IProductBaselinkerService
    {
        public Task<int> CreateMainAsync(ProductDTO product, MappingDTO mapping, CancellationToken cancellationToken);
        public Task<int> CreateVariantAsync(ProductDTO product, MappingDTO mapping, CancellationToken cancellationToken);
    }
}
