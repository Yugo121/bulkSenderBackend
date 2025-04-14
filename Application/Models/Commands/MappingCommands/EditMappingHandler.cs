using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Models.Commands.MappingCommands
{
    public class EditMappingHandler : IRequestHandler<EditMappingCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public EditMappingHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(EditMappingCommand request, CancellationToken cancellationToken)
        {
            Mapping mappingToUpdate = new()
            {
                Id = Guid.NewGuid(),
                Name = request.mapping.Name,
                Description = request.mapping.Description,
                Title = request.mapping.Title,
                CategoryId = request.mapping.Category.Id,
                BrandId = request.mapping.Brand.Id,
                MappingEntries = request.mapping.MappingEntriesDTO.Select(p => new MappingEntry
                {
                    Id = Guid.NewGuid(),
                    ColumnName = p.ColumnName,
                    MappingType = p.MappingType,
                    TargetField = p.TargetField,
                }).ToList()
            };
            _appDbContext.Mappings.Update(mappingToUpdate);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return mappingToUpdate.Id;
        }
    }
}
