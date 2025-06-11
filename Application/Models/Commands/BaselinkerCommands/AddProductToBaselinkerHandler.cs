using Application.Interfaces;
using Application.Models.DTOs;
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

            var isInBaselinker = productGroup.FirstOrDefault(p => p.Sku == request.product.Sku).IsAddedToBaselinker;

            if (!isInBaselinker)
            {
                ProductDTO mainProduct = request.product;
                mainProduct.Sku = _productPreparationService.ExtractMainSku(request.product.Sku);

                var productPayload = _productPreparationService.PrepareProduct(mainProduct, mapping);

                int parentProductId = await _baselinkerService.SendProductToBaselinker(productPayload, cancellationToken);

                newProductsIds.Add(parentProductId);

                foreach(var item in productGroup)
                    item.BaselinkerParentId = parentProductId;

                await _productGroupingService.UpdateParentIdAsync(productGroup, parentProductId, cancellationToken);
            }

            foreach (var product in productGroup)
            {
                var isSingleVariant = productGroup.Count == 1 && !product.Sku.Contains("_OS");

                if (product.IsAddedToBaselinker && product.BaselinkerParentId != 0 && !isSingleVariant)
                    continue;

                var productPayload = _productPreparationService.PrepareProduct(product, mapping);

                var productId = await _baselinkerService.SendProductToBaselinker(productPayload, cancellationToken);

                newProductsIds.Add(productId);

                await _productGroupingService.SetProductBaselinkerFlagAndId(product, productId, cancellationToken);
            }

            return newProductsIds;
        }
    }
}
