using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Models.Commands.BrandCommands
{
    public class AddBrandHandler : IRequestHandler<AddBrandCommand, Guid>
    {
        private readonly IAppDbContext _appDbContext;
        public AddBrandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Guid> Handle(AddBrandCommand request, CancellationToken cancellationToken)
        {
            Brand brand = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Brand.Name,
                BaselinkerId = request.Brand.BaselinkerId,
                Description = request.Brand.Description
            };

            await _appDbContext.Brands.AddAsync(brand);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return brand.Id;
        }
    }
}
