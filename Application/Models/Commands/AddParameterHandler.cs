using Domain.Entities;
using Infrastructure.Data;
using MediatR;

namespace Application.Models.Commands
{
    public class AddParameterHandler : IRequestHandler<AddParameterCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public AddParameterHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(AddParameterCommand request, CancellationToken cancellationToken)
        {
            Parameter parameter = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Parameter.Name,
                Value = request.Parameter.Value,
                BaselinkerId = request.Parameter.BaselinkerId
            };

            await _appDbContext.Parameters.AddAsync(parameter);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return parameter.Id;
        }
    }
}