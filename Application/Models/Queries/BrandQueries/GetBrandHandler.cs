using Application.Models.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Application.Models.Queries.BrandQueries
{
    public class GetBrandHandler : IRequestHandler<GetBrandQuery, BrandDTO>
    {
        private readonly IAppDbContext _appDbContext;
        public GetBrandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<BrandDTO> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            BrandDTO brand = await _appDbContext.Brands
                .Where(b => b.Id == request.id)
                .Select(b => new BrandDTO(b))
                .FirstOrDefaultAsync(cancellationToken);

            return brand;
        }
    }
}
