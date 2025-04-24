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
            List<string> brandNamesInDb =  _appDbContext.Brands
                .Where(b => brandNames.Contains(b.Name))
                .Select(b => b.Name)
                .ToList();
            List<string> newBrandNames = brandNames.Except(brandNamesInDb).ToList();

            foreach(var brandName in newBrandNames)
            {
                Brand brand = new()
                {
                    Id = new Guid(),
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
            List<string> categoriesNames = products.Select(p => p.Category.Name).Distinct().ToList();
            List<string> categoriesNamesInDb =  _appDbContext.Categories
                .Where(c => categoriesNames.Contains(c.Name))
                .Select(c => c.Name)
                .ToList();
            List<string> newCategoriesNames = categoriesNames.Except(categoriesNamesInDb).ToList();

            foreach (var categoryName in newCategoriesNames)
            {
                Category category = new()
                {
                    Id = new Guid(),
                    Name = categoryName,
                    BaselinkerId = products.Where(p => p.Category.Name == categoryName)
                                    .Select(p => p.BaselinkerId)
                                    .FirstOrDefault()
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
            List<ParameterDTO> parametersInDb =  _appDbContext.Parameters
                .Select(p => new ParameterDTO(p))
                .ToList(); 
            List<ParameterDTO> newParameters = productParameters
                .Where(p => !parametersInDb.Any(dbParam => dbParam.Name == p.Name && dbParam.Value == p.Value))
                .DistinctBy(p => new { p.Name, p.Value })
                .ToList();
            List<Parameter> paramsToAdd = new();

            foreach (var parameterName in newParameters)
            {
                Parameter parameter = new()
                {
                    Id = new Guid(),
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
            List<string> eansInDb = _appDbContext.Products
                .Select(p => p.Ean)
                .ToList();
            List<ProductDTO> newProducts = products.Where(p => !eansInDb.Contains(p.Ean)).ToList();

            List<Product> productsToAdd = new();
            try
            {
                foreach (var dto in newProducts)
                {
                    var matchedParameters = await _appDbContext.Parameters
                        .Where(p => dto.Parameters.Select(pd => pd.Name).Contains(p.Name))
                        .ToListAsync();

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
                        Description = dto.Description,
                        Price = dto.Price,
                        CategoryId = await _appDbContext.Categories
                        .Where(c => c.Name == dto.Category.Name)
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync(),
                        BrandId =  await _appDbContext.Brands
                        .Where(b => b.Name == dto.Brand.Name)
                        .Select(b => b.Id)
                        .FirstOrDefaultAsync(),
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
