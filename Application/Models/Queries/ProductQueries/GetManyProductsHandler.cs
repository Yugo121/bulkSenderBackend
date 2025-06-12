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
            var entities = await _appDbContext.Products
                           .OrderBy(p => p.Id)
                           .Skip((request.page - 1) * request.quantity)
                           .Take(request.quantity)
                           .Include(p => p.Category)
                               .ThenInclude(c => c.Aliases)
                           .Include(p => p.Brand)
                           .Include(p => p.Parameters)
                           .AsSplitQuery()
                           .AsNoTracking()
                           .ToListAsync(cancellationToken);

            List<ProductDTO> products = entities.Select(e => new ProductDTO(e)).ToList();
            return products;
        }
    }
}