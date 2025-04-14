using Application.Interfaces;
using Application.Models.DTO_s;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.MappingQueries
{
    public class GetMappingHandler : IRequestHandler<GetMappingQuery, MappingDTO>
    {
        private readonly IAppDbContext _appDbContext;
        public GetMappingHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<MappingDTO> Handle(GetMappingQuery request, CancellationToken cancellationToken)
        {
            var mapping = await _appDbContext.Mappings.FirstOrDefaultAsync(m => m.Id == request.id);

            if (mapping == null)
            {
                throw new Exception("Mapping not found");
            }

            MappingDTO mappingDTO = new(mapping);

            return mappingDTO;
        }
    }
}