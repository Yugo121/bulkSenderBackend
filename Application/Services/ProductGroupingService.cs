using Application.Interfaces;
using Application.Models.DTO_s;
using Application.Models.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ProductGroupingService : IProductGroupingService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IProductPreparationService _productPreparationService;
        public ProductGroupingService(IAppDbContext appDbContext, IProductPreparationService productPreparationService)
        {
            _appDbContext = appDbContext;
            _productPreparationService = productPreparationService;
        }
        public async Task<(string mainSku, List<ProductDTO> children)> GroupProductsAsync(ProductDTO product, CancellationToken cancellationToken)
        {
            string mainSku = _productPreparationService.ExtractMainSku(product.Sku);
            var entities = await _appDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Parameters)
                .Where(p => p.Sku.Contains(mainSku))
                .ToListAsync();

            var dtos = entities.Select(p => MapToDTO(p)).ToList();

            return (mainSku, dtos);

        }

        public async Task UpdateParentIdAsync(List<ProductDTO> children, int parentId, CancellationToken cancellationToken)
        {
            foreach(var variant in children)
            {
                var product = _appDbContext.Products.FirstOrDefault(p => p.Id == variant.Id);
                if (product != null)
                {
                    product.BaselinkerParentId = parentId;
                    _appDbContext.Products.Update(product);
                }
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private ProductDTO MapToDTO(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                BaselinkerParentId = product.BaselinkerParentId,
                BaselinkerId = product.BaselinkerId,
                Sku = product.Sku,
                Ean = product.Ean,
                Name = product.Name,
                IsAddedToBaselinker = product.IsAddedToBaselinker,
                Description = product.Description,
                Price = product.Price,
                Category = new CategoryDTO(product.Category),
                Brand = new BrandDTO(product.Brand),
                Parameters = product.Parameters.Select(p => new ParameterDTO(p)).ToList()
            };
        }

    }
}
