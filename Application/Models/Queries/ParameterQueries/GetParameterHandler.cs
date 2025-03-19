using Application.Models.DTOs;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Queries.ParameterQueries
{
    public class GetParameterHandler : IRequestHandler<GetParameterQuery, ParameterDTO>
    {
        private readonly IAppDbContext _appDbContext;
        public GetParameterHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ParameterDTO> Handle(GetParameterQuery request, CancellationToken cancellationToken)
        {
            ParameterDTO parameter = await _appDbContext.Parameters
                .Where(p => p.Id == request.Id)
                .Select(p => new ParameterDTO(p))
                .FirstOrDefaultAsync(cancellationToken);

            return parameter;
        }
    }
}
