using Domain.Entities;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Commands.BrandCommands
{
    public class EditBrandHandler : IRequestHandler<EditBrandCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public EditBrandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(EditBrandCommand request, CancellationToken cancellationToken)
        {
            Brand brand = await _appDbContext.Brands.FirstOrDefaultAsync(b => b.Id == request.Brand.Id);

            brand.Name = request.Brand.Name;
            brand.BaselinkerId = request.Brand.BaselinkerId;
            brand.Description = request.Brand.Description;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return brand.Id;
        }
    }
}
