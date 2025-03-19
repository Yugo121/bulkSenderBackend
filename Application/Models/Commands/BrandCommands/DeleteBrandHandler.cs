using Domain.Entities;
using Application.Interfaces;
using MediatR;

namespace Application.Models.Commands.BrandCommands
{
    public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public DeleteBrandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            Brand brand = await _appDbContext.Brands.FindAsync(request.BrandId);

            _appDbContext.Brands.Remove(brand);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return brand.Id;
        }
    }
}
