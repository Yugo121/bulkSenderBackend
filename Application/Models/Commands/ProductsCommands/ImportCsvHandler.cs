using Application.Interfaces;
using Application.Models.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.Models.Commands.ProductsCommands
{
    public class ImportCsvHandler : IRequestHandler<ImportCsvCommand>
    {
        private readonly ICsvProductParser _csvProductParser;
        private readonly IAppDbContext _appDbContext;

        public ImportCsvHandler(ICsvProductParser csvProductParser, IAppDbContext appDbContext)
        {
            _csvProductParser = csvProductParser;
            _appDbContext = appDbContext;
        }
            
        public async Task Handle(ImportCsvCommand request, CancellationToken cancellationToken)
        {
            var command = _csvProductParser.ParseCsv(request.csvStream, request.columnMappings);

            List<Product> products = new();

            foreach (var product in command.Products)
            {
                products.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    BaselinkerId = product.BaselinkerId,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    Sku = product.Sku,
                    Ean = product.Ean,
                    MainImage = product.MainImage,
                    SecondImage = product.SecondImage,
                    ThirdImage = product.ThirdImage,
                    CategoryId = product.Category.Id,
                    BrandId = product.Brand.Id,
                    Parameters = product.Parameters.Select(p => new Parameter
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Value = p.Value
                    }).ToList()
                });
            }
            await _appDbContext.Products.AddRangeAsync(products, cancellationToken);
            await _appDbContext.SaveChangesAsync(cancellationToken);

        }
    }
}
