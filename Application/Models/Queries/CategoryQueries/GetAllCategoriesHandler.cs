using Application.Interfaces;
using Application.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.CategoryQueries
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllCategoriesHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<CategoryDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            List<CategoryDTO> categories = await _appDbContext.Categories
                .Include(c => c.Aliases)
                .Select(c => new CategoryDTO(c))
                .ToListAsync(cancellationToken);

            return categories;
        }
    }
}
