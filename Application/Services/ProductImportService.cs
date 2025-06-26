using Application.Interfaces;
using Application.Models.DTO_s;
using Application.Models.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ProductImportService : IProductImportService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ICsvProductParser _csvProductParser;

        public ProductImportService(IAppDbContext appDbContext, ICsvProductParser csvProductParser)
        {
            _appDbContext = appDbContext;
            _csvProductParser = csvProductParser;
        }

        public async Task ImportAsync(CsvImportRequest importRequest, CancellationToken cancellationToken)
        {
            var productDTOs = await _csvProductParser.ParseCsv(importRequest);

            await ImportBrandsAsync(productDTOs, cancellationToken);
            await ImportCategoriesAsync(productDTOs, cancellationToken);
            await ImportParametersAsync(productDTOs, cancellationToken);
            await ImportProductsAsync(productDTOs, cancellationToken);
        }

        private async Task ImportBrandsAsync(List<ProductDTO> products, CancellationToken cancellationToken)
        {
            List<string> brandNames = products.Select(p => p.Brand.Name).Distinct().ToList();

            List<string> brandNamesInDb = await _appDbContext.Brands
                    .Where(b => brandNames.Contains(b.Name))
                    .Select(b => b.Name)
                    .ToListAsync(cancellationToken);
            List<string> newBrandNames = brandNames.Except(brandNamesInDb).ToList();

            foreach(var brandName in newBrandNames)
            {
                Brand brand = new()
                {
                    Id = Guid.NewGuid(),
                    Name = brandName,
                    BaselinkerId = 0,
                    Description = "Brand description",
                };
                _appDbContext.Brands.Add(brand);
            }
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ImportCategoriesAsync(List<ProductDTO> products, CancellationToken cancellationToken)
        {
            var distinctCategoriesFromProducts = products.Select(p => p.Category).DistinctBy(cat =>
        string.Join("|",
            cat.Aliases
               .Select(a => a.Name)
               .OrderBy(n => n)
        )).ToList();
            var categoriesAliases = distinctCategoriesFromProducts.SelectMany(c => c.Aliases.Select(ca => ca.Name)).Distinct().ToList();

            var baselinkerIds = distinctCategoriesFromProducts.Select(c => c.BaselinkerId).Distinct().ToList();

            var categoriesInDb = await _appDbContext.Categories.Include(c => c.Aliases)
                .Where(c => baselinkerIds.Contains(c.BaselinkerId)).ToListAsync(cancellationToken);

            var existingAliasNames = new HashSet<string>(categoriesInDb
                .SelectMany(c => c.Aliases.Select(a => a.Name)));

            var newCategories = distinctCategoriesFromProducts
                .Where(cat => cat.Aliases.Any(alias => !existingAliasNames.Contains(alias.Name)))
                .ToList();


            foreach (var cat in newCategories)
            {
                var existingCategory = categoriesInDb.FirstOrDefault(c => c.BaselinkerId == cat.BaselinkerId);

                if (existingCategory != null)
                {
                    foreach (var alias in cat.Aliases)
                    {
                        if (!existingCategory.Aliases.Any(a => a.Name == alias.Name))
                        {
                            _appDbContext.CategoryAliases.Add(new CategoryAlias
                            {
                                Id = Guid.NewGuid(),
                                Name = alias.Name,
                                CategoryId = existingCategory.Id
                            });
                        }  
                    }
                    await _appDbContext.SaveChangesAsync(cancellationToken);

                    continue;
                }
                Category category = new()
                {
                    Id = Guid.NewGuid(),
                    Aliases = cat.Aliases.Select(alias => new CategoryAlias
                    {
                        Id = Guid.NewGuid(),
                        Name = alias.Name,
                    }).ToList(),
                    BaselinkerId = cat.BaselinkerId,
                    BaselinkerName = cat.BaselinkerName,
                };
                _appDbContext.Categories.Add(category);
            }

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ImportParametersAsync(List<ProductDTO> products, CancellationToken cancellationToken)
        {
            List<ParameterDTO> productParameters = products.Select(p => p.Parameters)
                .SelectMany(p => p)
                .Distinct()
                .ToList();
            List<ParameterDTO> parametersInDb = await _appDbContext.Parameters
                .Select(p => new ParameterDTO(p))
                .ToListAsync(cancellationToken); 
            List<ParameterDTO> newParameters = productParameters
                .Where(p => !parametersInDb.Any(dbParam => dbParam.Name == p.Name && dbParam.Value == p.Value))
                .DistinctBy(p => new { p.Name, p.Value })
                .ToList();
            List<Parameter> paramsToAdd = new();

            foreach (var parameterName in newParameters)
            {
                Parameter parameter = new()
                {
                    Id = Guid.NewGuid(),
                    Name = parameterName.Name,
                    Value = parameterName.Value,
                    BaselinkerId = 0
                };
                paramsToAdd.Add(parameter);
            }
            _appDbContext.Parameters.AddRange(paramsToAdd);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ImportProductsAsync(List<ProductDTO> products, CancellationToken cancellationToken)
        {
            List<string> eansInDb = await _appDbContext.Products
                .Select(p => p.Ean)
                .ToListAsync(cancellationToken);
            List<ProductDTO> newProducts = products.Where(p => !eansInDb.Contains(p.Ean)).ToList();

            List<Product> productsToAdd = new();
            try
            {
                foreach (var dto in newProducts)
                {
                    var matchedParameters = await _appDbContext.Parameters
                        .Where(p => dto.Parameters.Select(pd => pd.Name).Contains(p.Name))
                        .ToListAsync(cancellationToken);

                    var productParameters = matchedParameters
                        .Select(p => new Parameter
                        {
                            Name = p.Name,
                            Value = dto.Parameters.First(pd => pd.Name == p.Name).Value
                        }).DistinctBy(p => new { p.Name, p.Value })
                        .ToList();


                    Product product = new()
                    {
                        Id = dto.Id,
                        Sku = dto.Sku,
                        Ean = dto.Ean,
                        Name = dto.Name,
                        Description = dto.Description ?? "",
                        IsAddedToBaselinker = dto.IsAddedToBaselinker,
                        Price = dto.Price,
                        CategoryId = await _appDbContext.Categories
                        .Where(c => c.BaselinkerId == dto.Category.BaselinkerId)
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync(cancellationToken),
                        BrandId =  await _appDbContext.Brands
                        .Where(b => b.Name == dto.Brand.Name)
                        .Select(b => b.Id)
                        .FirstOrDefaultAsync(cancellationToken),
                        Parameters = productParameters
                    };
                    productsToAdd.Add(product);
                }

                _appDbContext.Products.AddRange(productsToAdd);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: ", ex.Message);
            }
        }

    }
}
