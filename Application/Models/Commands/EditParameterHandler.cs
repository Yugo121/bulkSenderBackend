using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands
{
    public class EditParameterHandler : IRequestHandler<EditParameterCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public EditParameterHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(EditParameterCommand request, CancellationToken cancellationToken)
        {
            Parameter parameter = await _appDbContext.Parameters.FirstOrDefaultAsync(p => p.Id == request.Parameter.Id);

            parameter.Name = request.Parameter.Name;
            parameter.Value = request.Parameter.Value;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return parameter.Id;
        }
    }
}
