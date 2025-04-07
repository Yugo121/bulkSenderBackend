using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Queries.ParameterQueries
{
    public class GetParametersNamesHandler : IRequestHandler<GetParametersNamesQuery, List<string>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetParametersNamesHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<string>> Handle(GetParametersNamesQuery request, CancellationToken cancellationToken)
        {
            List<string> parametersNames = new();

            parametersNames = await _appDbContext.Parameters
                .Select(p => p.Name)
                .ToListAsync(cancellationToken);

            return parametersNames;
        }
    }
}
