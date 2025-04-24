using Application.Models.DTOs;

namespace Application.Interfaces
{
    public interface IBaselinkerService
    {
        Task<int> SendProductToBaselinker(ProductDTO product, CancellationToken cancellationToken);
        Task<string> GetCategories(CancellationToken cancellationToken);
        Task<string> GetBrands(CancellationToken cancellationToken);

    }
}
