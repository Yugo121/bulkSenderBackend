using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands.MappingCommands
{
    public class DeleteMappingHandler : IRequestHandler<DeleteMappingCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public DeleteMappingHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Handle(DeleteMappingCommand request, CancellationToken cancellationToken)
        {
            Mapping mappingToDelete = await _appDbContext.Mappings.FirstOrDefaultAsync(m => m.Id == request.id);

            if(mappingToDelete == null)
            {
                throw new Exception("Mapping not found");
            }

            _appDbContext.Mappings.Remove(mappingToDelete);

            return mappingToDelete.Id;
        }
    
    }
}
