using Domain.Entities;
using Application.Interfaces;
using MediatR;

namespace Application.Models.Commands.CategoryCommands
{
    internal class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public DeleteCategoryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = await _appDbContext.Categories.FindAsync(request.CategoryId);

            _appDbContext.Categories.Remove(category);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}
