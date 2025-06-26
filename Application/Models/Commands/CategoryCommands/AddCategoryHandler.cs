using Application.Models.DTOs;
using Domain.Entities;
using Application.Interfaces;
using MediatR;

namespace Application.Models.Commands.CategoryCommands
{
    public class AddCategoryHandler : IRequestHandler<AddCategoryCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public AddCategoryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = new()
            {
                Id = Guid.NewGuid(),
                Aliases = request.category.Aliases.Select(a => new CategoryAlias
                {
                    Id = Guid.NewGuid(),
                    Name = a.Name,
                    CategoryId = a.CategoryId
                }).ToList(),
                BaselinkerId = request.category.BaselinkerId,
                BaselinkerName = request.category.BaselinkerName,
            };

            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}
