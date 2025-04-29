using Application.Interfaces;
using MediatR;

namespace Application.Models.Commands.BaselinkerCommands
{
    public class AddProductToBaselinkerHandler : IRequestHandler<AddProductToBaselinkerCommand, List<int>>
    {
        private readonly IBaselinkerService _baselinkerService;
        private readonly IProductGroupingService _productGroupingService;
        private readonly IProductPreparationService _productPreparationService;

        public AddProductToBaselinkerHandler(IBaselinkerService baselinkerService, IProductPreparationService productPreparationService, IProductGroupingService productGroupingService)
        {
            _baselinkerService = baselinkerService;
            _productPreparationService = productPreparationService;
            _productGroupingService = productGroupingService;
        }
        public async Task<List<int>> Handle(AddProductToBaselinkerCommand request, CancellationToken cancellationToken)
        {
            List<int> newProductsIds = new List<int>();

            var productGroup = await _productGroupingService.GroupProductsAsync(request.product, cancellationToken);
            var mapping = await _productGroupingService.GetMappingForProduct(request.product, cancellationToken);

            if (request.product.BaselinkerParentId == 0 && !request.product.Sku.Contains("_OS"))
            {
                request.product.Sku = _productPreparationService.ExtractMainSku(request.product.Sku);

                var productPayload = _productPreparationService.PrepareProduct(request.product, mapping);

                Console.WriteLine($"Product: {productPayload}");

                int parentProductId = await _baselinkerService.SendProductToBaselinker(productPayload, cancellationToken);

                newProductsIds.Add(parentProductId);

                await _productGroupingService.UpdateParentIdAsync(productGroup, parentProductId, cancellationToken);
            } else
            {
                foreach (var product in productGroup)
                {
                    if(product.IsAddedToBaselinker)
                        continue;

                    var productPayload = _productPreparationService.PrepareProduct(product, mapping);

                    await _productGroupingService.SetProductBaselinkerFlag(product, cancellationToken);

                    newProductsIds.Add(await _baselinkerService.SendProductToBaselinker(productPayload, cancellationToken));
                }
            }

            return newProductsIds;
        }
    }
}
