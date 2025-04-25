using Application.Interfaces;
using Application.Models.DTO_s;
using Application.Models.DTOs;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands.BaselinkerCommands
{
    public class AddProductToBaselinkerHandler : IRequestHandler<AddProductToBaselinkerCommand, int>
    {
        private readonly IBaselinkerService _baselinkerService;
        private readonly IProductPreparationService _productPreparationService;
        private readonly IAppDbContext _appDbContext;

        public AddProductToBaselinkerHandler(IBaselinkerService baselinkerService, IProductPreparationService productPreparationService, IAppDbContext appDbContext)
        {
            _baselinkerService = baselinkerService;
            _productPreparationService = productPreparationService;
            _appDbContext = appDbContext;
        }
        public async Task<int> Handle(AddProductToBaselinkerCommand request, CancellationToken cancellationToken)
        {
            string mainSKu = _productPreparationService.ExtractMainSku(request.product.Sku);

            var productGroup = await _appDbContext.Products.Where(p => p.Sku.Contains(mainSKu)).ToListAsync(cancellationToken);

            MappingDTO mappingForProduct = new(await _appDbContext.Mappings
                .FirstOrDefaultAsync(m => m.CategoryId == request.product.Category.Id && m.BrandId == request.product.Brand.Id));

            if (request.product.BaselinkerParentId == 0 && !request.product.Sku.Contains("_OS"))
            {
                var productToSend = _productPreparationService.PrepareProduct(request.product, mappingForProduct);

                int result = await _baselinkerService.SendProductToBaselinker(productToSend, cancellationToken);

                foreach (var item in productGroup)
                {
                    item.BaselinkerParentId = result;
                    _appDbContext.Products.Update(item);
                }
                await _appDbContext.SaveChangesAsync(cancellationToken);

                return result;
            } else
            {
                List<ProductDTO> productsToSend = productGroup.Select(p => new ProductDTO
                {
                    BaselinkerParentId = p.BaselinkerParentId,
                    Id = p.Id,
                    Sku = p.Sku,
                    Ean = p.Ean,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Category = new CategoryDTO
                    {
                        Id = p.CategoryId,
                        Name = p.Category.Name,
                        BaselinkerId = p.Category.BaselinkerId
                    },
                    Brand = new BrandDTO
                    {
                        Id = p.BrandId,
                        Name = p.Brand.Name,
                        BaselinkerId = p.Brand.BaselinkerId,
                        Description = p.Brand.Description
                    },
                    Parameters = p.Parameters.Select(pr => new ParameterDTO
                    {
                        Id = pr.Id,
                        Name = pr.Name,
                        Value = pr.Value,
                        BaselinkerId = pr.BaselinkerId
                    }).ToList()
                }).ToList();

                int result = 0;

                foreach (var product in productsToSend)
                {
                   var item = _productPreparationService.PrepareProduct(product, mappingForProduct);

                   result = await _baselinkerService.SendProductToBaselinker(item, cancellationToken);

                }

                return result;
            }

        }
    }
}
