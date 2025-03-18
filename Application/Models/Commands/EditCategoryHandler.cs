using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands
{
    public class EditCategoryHandler : IRequestHandler<EditCategoryCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public EditCategoryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Guid> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.Id == request.Category.Id);

            category.Name = request.Category.Name;
            category.BaselinkerId = request.Category.BaselinkerId;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return category.Id;
        }
    }
}
