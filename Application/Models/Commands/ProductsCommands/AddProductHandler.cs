using Domain.Entities;
using Application.Interfaces;
using MediatR;

namespace Application.Models.Commands.ProductsCommands
{
    public class AddProductHandler : IRequestHandler<AddProductCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public AddProductHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new()
            {
                Id = Guid.NewGuid(),
                BaselinkerId = request.Product.BaselinkerId,
                Name = request.Product.Name,
                Price = request.Product.Price,
                Description = request.Product.Description,
                Sku = request.Product.Sku,
                Ean = request.Product.Ean,
                CategoryId = request.Product.Category.Id,
                BrandId = request.Product.Brand.Id,
                Parameters = request.Product.Parameters.Select(p => new Parameter
                {
                    Id = Guid.NewGuid(),
                    Name = p.Name,
                    Value = p.Value
                }).ToList()
            };

            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
