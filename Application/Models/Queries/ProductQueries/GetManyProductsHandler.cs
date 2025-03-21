using Application.Interfaces;
using Application.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.ProductQueries
{
    public class GetManyProductsHandler : IRequestHandler<GetManyProductsQuery, List<ProductDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetManyProductsHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<ProductDTO>> Handle(GetManyProductsQuery request, CancellationToken cancellationToken)
        {
            List<ProductDTO> products = await _appDbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Parameters)
                .Skip(request.page - 1 * request.quantity)
                .Take(request.quantity)
                .Select(p => new ProductDTO(p))
                .ToListAsync();

            return products;
        }
    }
}
