using Application.Interfaces;
using MediatR;

namespace Application.Models.Queries.BaselinkerQueries
{
    public class GetBaselinkerCategoriesHandler : IRequestHandler<GetBaselinkerCategoriesQuery, string>
    {
        private readonly IBaselinkerService _baselinkerService;
        public GetBaselinkerCategoriesHandler(IBaselinkerService baselinkerService)
        {
            _baselinkerService = baselinkerService;
        }
        public async Task<string> Handle(GetBaselinkerCategoriesQuery request, CancellationToken cancellationToken)
        {
            string result = await _baselinkerService.GetCategories(cancellationToken);
            return result;
        }
    }
}
