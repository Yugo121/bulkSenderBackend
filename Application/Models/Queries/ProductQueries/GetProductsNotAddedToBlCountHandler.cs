using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.ProductQueries
{
    public class GetProductsNotAddedToBlCountHandler : IRequestHandler<GetProductsNotAddedToBlCount, int>
    {
        private readonly IAppDbContext _appDbContext;
        public GetProductsNotAddedToBlCountHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<int> Handle(GetProductsNotAddedToBlCount request, CancellationToken cancellationToken)
        {
            int count = await _appDbContext.Products
                .Where(p => !p.IsAddedToBaselinker)
                .CountAsync(cancellationToken);

            return count;
        }
    }
}
