using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.MappingQueries
{
    internal class GetMappingsNamesHandler : IRequestHandler<GetMappingsNamesQuery, List<string>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetMappingsNamesHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<string>> Handle(GetMappingsNamesQuery request, CancellationToken cancellationToken)
        {
            List<string> mappingNames = new();

            mappingNames = await _appDbContext.Mappings
                .Select(m => m.Name)
                .ToListAsync();

            return mappingNames;
        }
    }
}
