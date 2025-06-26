using Application.Models.DTO_s;

namespace Application.Interfaces
{
    public interface IBaselinkerService
    {
        Task<int> SendProductToBaselinker(ProductToBaselinkerDTO product, CancellationToken cancellationToken);
        Task<string> GetCategories(CancellationToken cancellationToken);
        Task<string> GetBrands(CancellationToken cancellationToken);
    }
}
