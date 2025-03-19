using Domain.Entities;
using Application.Interfaces;
using MediatR;

namespace Application.Models.Commands.ParameterCommands
{
    public class DeleteParameterHandler : IRequestHandler<DeleteParameterCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public DeleteParameterHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(DeleteParameterCommand request, CancellationToken cancellationToken)
        {
            Parameter parameter = await _appDbContext.Parameters.FindAsync(request.ParameterId);
   
            _appDbContext.Parameters.Remove(parameter);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return parameter.Id;
        }
    }
}
