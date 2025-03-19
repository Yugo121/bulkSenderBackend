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
                Name = request.category.Name,
                BaselinkerId = request.category.BaselinkerId
            };

            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}
