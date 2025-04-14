using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Mapping> Mappings { get; set; }
        public DbSet<MappingEntry> MappingEntries { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
