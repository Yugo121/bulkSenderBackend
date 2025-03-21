using Application.Interfaces;
using Application.Models.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.ParameterQueries
{
    public class GetAllParametersHandler : IRequestHandler<GetAllParametersQuery, List<ParameterDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllParametersHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<ParameterDTO>> Handle(GetAllParametersQuery request, CancellationToken cancellationToken)
        {
            List<ParameterDTO> parameters = await _appDbContext.Parameters
                .Select(p => new ParameterDTO(p))
                .ToListAsync(cancellationToken);

            return parameters;
        }
    }
}
