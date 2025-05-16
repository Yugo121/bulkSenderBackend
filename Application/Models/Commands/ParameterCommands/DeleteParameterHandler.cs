using Domain.Entities;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands.ParameterCommands
{
    public class DeleteParameterHandler : IRequestHandler<DeleteParameterCommand, int>
    {
        private readonly IAppDbContext _appDbContext;
        public DeleteParameterHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<int> Handle(DeleteParameterCommand request, CancellationToken cancellationToken)
        {
            int deletedCount = await _appDbContext.Parameters.Where(p => p.Name == request.Name).ExecuteDeleteAsync(cancellationToken);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return deletedCount;
        }
    }
}
