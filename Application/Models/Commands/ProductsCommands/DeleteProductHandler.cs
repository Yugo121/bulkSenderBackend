using Domain.Entities;
using Application.Interfaces;
using MediatR;

namespace Application.Models.Commands.ProductsCommands
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public DeleteProductHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Product product = await _appDbContext.Products.FindAsync(request.ProductId);

            _appDbContext.Products.Remove(product);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
