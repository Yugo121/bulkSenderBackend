using Application.Interfaces;
using Application.Models.DTO_s;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.MappingQueries
{
    public class GetMappingByNameHandler : IRequestHandler<GetMappingByNameQuery, MappingDTO>
    {
        private readonly IAppDbContext _appDbContext;
        public GetMappingByNameHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<MappingDTO> Handle(GetMappingByNameQuery request, CancellationToken cancellationToken)
        {
            Mapping searchedMapping = await _appDbContext.Mappings
                .Include(m => m.MappingEntries)
                .Include(m => m.Brand)
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Name == request.name, cancellationToken);

            if (searchedMapping == null)
            {
                throw new Exception($"Mapping with name {request.name} not found");
            }

            MappingDTO mapping = new(searchedMapping);

            return mapping;

        }
    }
}
