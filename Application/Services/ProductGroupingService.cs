using Application.Interfaces;
using Application.Models.DTO_s;
using Application.Models.DTOs;
using Domain.Entities;
using MediatR;
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
        public async Task<List<ProductDTO>> GroupProductsAsync(ProductDTO product, CancellationToken cancellationToken)
        {
            string mainSku = _productPreparationService.ExtractMainSku(product.Sku);

            var entities = await _appDbContext.Products
                .AsNoTracking()
                .Where(p => p.Sku.StartsWith(mainSku))
                .Include(p => p.Category)
                    .ThenInclude(c => c.Aliases)
                .Include(p => p.Brand)
                .Include(p => p.Parameters)
                .AsSplitQuery()
                .ToListAsync();

            var dtos = entities.Select(p => MapToDTO(p)).ToList();

            return dtos;

        }

        public async Task<MappingDTO> GetMappingForProduct(ProductDTO product, CancellationToken cancellationToken)
        {
            var mapping = await _appDbContext.Mappings
                .Include(m => m.Category)
                    .ThenInclude(c => c.Aliases)
                .Include(m => m.Brand)
                .Include(m => m.MappingEntries)
                .FirstOrDefaultAsync(m => m.CategoryId == product.Category.Id && m.BrandId == product.Brand.Id, cancellationToken);

            if (mapping == null)
                throw new InvalidOperationException($"Brak mapowania dla kategorii {product.Category.Id} i marki {product.Brand.Id}");

            return new MappingDTO
            {
                Id = mapping.Id,
                Category = new CategoryDTO(mapping.Category),
                Brand = new BrandDTO(mapping.Brand),
                Description = mapping.Description,
                Title = mapping.Title,
                Name = mapping.Name,
                MappingEntriesDTO = mapping.MappingEntries
                    .Select(me => new MappingEntryDTO
                    {
                        Id = me.Id,
                        ColumnName = me.ColumnName,
                        TargetField = me.TargetField,
                        MappingType = me.MappingType
                    }).ToList()
            };  
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
            }
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> SetProductBaselinkerFlagAndId(ProductDTO product, int id, CancellationToken cancellationToken)
        {
            var productEntity = await _appDbContext.Products
                .FirstOrDefaultAsync(p => p.Id == product.Id, cancellationToken);

            if (productEntity == null)
                return false;

            productEntity.IsAddedToBaselinker = true;
            productEntity.BaselinkerId = id;

            _appDbContext.Products.Update(productEntity);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return true;
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
