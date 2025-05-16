using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.CategoryQueries
{
    public class GetCategoriesNamesHandler : IRequestHandler<GetCategoriesNamesQuery, List<string>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetCategoriesNamesHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<string>> Handle(GetCategoriesNamesQuery request, CancellationToken cancellationToken)
        {
            //List<string> categoriesNames = await _appDbContext.Categories
            //    .Select(c => c.Name)
            //    .ToListAsync();
            List<string> categoriesNames = await _appDbContext.Categories
                .Select(c => c.Aliases.FirstOrDefault().Name)
                .ToListAsync(cancellationToken);

            return categoriesNames;
        }
    }
}
