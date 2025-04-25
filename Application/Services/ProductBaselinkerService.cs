using Application.Interfaces;
using Application.Models.DTO_s;
using Application.Models.DTOs;
using System.Threading;

namespace Application.Services
{
    public class ProductBaselinkerService : IProductBaselinkerService
    {
        private readonly IBaselinkerService _baselinkerService;
        private readonly IProductPreparationService _productPreparationService;
        public ProductBaselinkerService(IBaselinkerService baselinkerService, IProductPreparationService productPreparationService)
        {
            _baselinkerService = baselinkerService;
            _productPreparationService = productPreparationService;
        }

        //Czy ten serwis jest mi potrzebny? Może lepiej przenieść gdzieś indziej metode albo całokwicie ją wyrzucić
        public async Task<int> CreateMainAsync(ProductDTO product, MappingDTO mapping, CancellationToken cancellationToken)
        {
            var payload = _productPreparationService.PrepareProduct(product, mapping);
            int result = await _baselinkerService.SendProductToBaselinker(payload, cancellationToken);

            return result;
        }

        public Task<int> CreateVariantAsync(ProductDTO product, MappingDTO mapping, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
