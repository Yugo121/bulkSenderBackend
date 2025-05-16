using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands.MappingCommands
{
    public class AddMappingHandler : IRequestHandler<AddMappingCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public AddMappingHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(AddMappingCommand request, CancellationToken cancellationToken)
        {
            Brand brand =  await _appDbContext.Brands.FirstOrDefaultAsync(b => b.Name == request.mapping.Brand.Name, cancellationToken);
            Category category = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Id == request.mapping.Category.Id, cancellationToken);

            Mapping mapping = new()
            {
                Id = Guid.NewGuid(),
                Name = request.mapping.Name,
                Description = request.mapping.Description,
                Title = request.mapping.Title,
                CategoryId = category.Id,
                BrandId = brand.Id,
                MappingEntries = request.mapping.MappingEntriesDTO.Select(p => new MappingEntry
                {
                    Id = Guid.NewGuid(),
                    MappingType = p.MappingType,
                    ColumnName = p.ColumnName,
                    TargetField = p.TargetField,
                }).ToList()
            };

            _appDbContext.MappingEntries.AddRange(mapping.MappingEntries);
            _appDbContext.Mappings.Add(mapping);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return mapping.Id;
        }
    }
}
