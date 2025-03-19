using Domain.Entities;
using Application.Interfaces;
using MediatR;

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

            foreach (var productDTO in request.Products)
            {
                Product product = new()
                {
                    Id = Guid.NewGuid(),
                    BaselinkerId = productDTO.BaselinkerId,
                    Name = productDTO.Name,
                    Price = productDTO.Price,
                    Description = productDTO.Description,
                    Sku = productDTO.Sku,
                    Ean = productDTO.Ean,
                    MainImage = productDTO.MainImage,
                    SecondImage = productDTO.SecondImage,
                    ThirdImage = productDTO.ThirdImage,
                    CategoryId = productDTO.Category.Id,
                    BrandId = productDTO.Brand.Id,
                    Parameters = productDTO.Parameters.Select(p => new Parameter
                    {
                        Id = Guid.NewGuid(),
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
