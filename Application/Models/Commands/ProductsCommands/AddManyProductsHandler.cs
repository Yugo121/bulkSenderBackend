using Domain.Entities;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands.ProductsCommands
{
    public class AddManyProductsHandler : IRequestHandler<AddManyProductsCommand, List<Guid>>
    {
        private readonly IAppDbContext _appDbContext;
        public AddManyProductsHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<Guid>> Handle(AddManyProductsCommand request, CancellationToken cancellationToken)
        {
            List<Product> products = new();

            List<string> brandNames = request.Products
                  .Select(p => p.Brand.Name)
                  .Where(name => !string.IsNullOrWhiteSpace(name))
                  .Distinct()
                  .ToList();

            List<string> categoryNames = request.Products
                .Select(p => p.Category.BaselinkerName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .ToList();

            Dictionary<string, Guid> brandDict = await _appDbContext.Brands
                .Where(b => brandNames.Contains(b.Name))
                .ToDictionaryAsync(b => b.Name, b => b.Id, cancellationToken);

            Dictionary<string, Guid> categoryDict = await _appDbContext.Categories
                .Where(c => categoryNames.Contains(c.BaselinkerName))
                .ToDictionaryAsync(c => c.BaselinkerName, c => c.Id, cancellationToken);

            foreach (var productDTO in request.Products)
            {
                if (!brandDict.TryGetValue(productDTO.Brand.Name, out var brandId))
                    throw new Exception($"Nie znaleziono marki: {productDTO.Brand.Name}");

                if (!categoryDict.TryGetValue(productDTO.Category.BaselinkerName, out var categoryId))
                    throw new Exception($"Nie znaleziono kategorii: {productDTO.Category.BaselinkerName}");
                if (productDTO.Parameters.Count <= 1 && productDTO.Parameters[0].Name == "")
                {
                    productDTO.Parameters.Clear();
                }

                Product product = new()
                {
                    Id = Guid.NewGuid(),
                    BaselinkerParentId = productDTO.BaselinkerParentId,
                    BaselinkerId = productDTO.BaselinkerId,
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    Description = productDTO.Description,
                    IsAddedToBaselinker = productDTO.IsAddedToBaselinker,
                    Sku = productDTO.Sku,
                    Ean = productDTO.Ean,
                    CategoryId = categoryId,
                    BrandId = brandId,
                    Parameters = productDTO.Parameters.Select(p => new Parameter
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Value = p.Value
                    }).ToList()
                };

                products.Add(product);
            }

            await _appDbContext.Products.AddRangeAsync(products);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return products.Select(p => p.Id).ToList();
        }
    }
}
