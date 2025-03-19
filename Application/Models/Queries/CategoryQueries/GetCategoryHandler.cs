using Application.Models.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.CategoryQueries
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO>
    {
        private readonly IAppDbContext _appDbContext;
        public GetCategoryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<CategoryDTO> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            CategoryDTO category = await _appDbContext.Categories
                .Where(c => c.Id == request.Id)
                .Select(c => new CategoryDTO(c))
                .FirstOrDefaultAsync(cancellationToken);

            return category;
        }
    }
}
