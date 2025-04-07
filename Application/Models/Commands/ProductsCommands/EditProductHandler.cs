using Domain.Entities;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands.ProductsCommands
{
    public class EditProductHandler : IRequestHandler<EditProductCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public EditProductHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            Product product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Product.Id);

            product.Name = request.Product.Name;
            product.BaselinkerId = request.Product.BaselinkerId;
            product.Description = request.Product.Description;
            product.Price = request.Product.Price;
            product.Sku = request.Product.Sku;
            product.Ean = request.Product.Ean;
            product.CategoryId = request.Product.Category.Id;
            product.BrandId = request.Product.Brand.Id;
            product.Parameters = request.Product.Parameters.Select(p => new Parameter
            {
                Id = Guid.NewGuid(),
                Name = p.Name,
                Value = p.Value
            }).ToList();

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
