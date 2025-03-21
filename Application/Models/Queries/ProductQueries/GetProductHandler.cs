using Application.Models.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.ProductQueries
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, ProductDTO>
    {
        private readonly IAppDbContext _appDbContext;
        public GetProductHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            ProductDTO product = await _appDbContext.Products
                .Where(p => p.Id == request.Id)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Parameters)
                .Select(p => new ProductDTO(p))
                .FirstOrDefaultAsync(cancellationToken);
           
            return product;
        }
    }
}
