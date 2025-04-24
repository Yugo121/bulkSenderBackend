using Application.Interfaces;
using MediatR;

namespace Application.Models.Commands.BaselinkerCommands
{
    public class AddProductToBaselinkerHandler : IRequestHandler<AddProductToBaselinkerCommand, int>
    {
        private readonly IBaselinkerService _baselinkerService;

        public AddProductToBaselinkerHandler(IBaselinkerService baselinkerService)
        {
            _baselinkerService = baselinkerService;
        }
        public async Task<int> Handle(AddProductToBaselinkerCommand request, CancellationToken cancellationToken)
        {
            int result = await _baselinkerService.SendProductToBaselinker(request.product, cancellationToken);

            return result;
        }
    }
}
