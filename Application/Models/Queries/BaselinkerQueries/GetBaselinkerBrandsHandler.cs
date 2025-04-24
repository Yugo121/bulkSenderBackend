using Application.Interfaces;
using MediatR;

namespace Application.Models.Queries.BaselinkerQueries
{
    public class GetBaselinkerBrandsHandler : IRequestHandler<GetBaselinkerBrandsQuery, string>
    {
        private readonly IBaselinkerService _baselinkerService;
        public GetBaselinkerBrandsHandler(IBaselinkerService baselinkerService)
        {
            _baselinkerService = baselinkerService;
        }
        public async Task<string> Handle(GetBaselinkerBrandsQuery request, CancellationToken cancellationToken)
        {
            string result = await _baselinkerService.GetBrands(cancellationToken);
            return result;
        }
    }
}
