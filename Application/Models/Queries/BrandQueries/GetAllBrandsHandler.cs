using Application.Interfaces;
using Application.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.BrandQueries
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, List<BrandDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllBrandsHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<BrandDTO>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            List<BrandDTO> brands = await _appDbContext.Brands
                .Select(b => new BrandDTO(b))
                .ToListAsync(cancellationToken);

            return brands;
        }
    }
}
