using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.BrandQueries
{
    public class GetBrandsNamesHandler : IRequestHandler<GetBrandsNamesQuery, List<string>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetBrandsNamesHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<string>> Handle(GetBrandsNamesQuery request, CancellationToken cancellationToken)
        {
            List<string> brandsNames = await _appDbContext.Brands
                .Select(b => b.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToListAsync(cancellationToken);

            return brandsNames;
        }
    }
}
