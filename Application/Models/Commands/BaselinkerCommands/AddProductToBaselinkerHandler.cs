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

            if (request.product.BaselinkerParentId == 0 && !request.product.Sku.Contains("_OS"))
            {
                ProductDTO mainProduct = request.product;
                mainProduct.Sku = _productPreparationService.ExtractMainSku(request.product.Sku);

                var productPayload = _productPreparationService.PrepareProduct(mainProduct, mapping);

                int parentProductId = await _baselinkerService.SendProductToBaselinker(productPayload, cancellationToken);

                newProductsIds.Add(parentProductId);

                foreach(var item in productGroup)
                    item.BaselinkerParentId = parentProductId;

                //poprawić tak aby tworzyło od razu produkt głowny i warianty.
                await _productGroupingService.UpdateParentIdAsync(productGroup, parentProductId, cancellationToken);
            }

            foreach (var product in productGroup)
            {
                if (product.IsAddedToBaselinker && product.BaselinkerParentId != 0)
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
