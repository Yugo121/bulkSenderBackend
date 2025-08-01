using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryAlias> CategoryAliases { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Mapping> Mappings { get; set; }
        public DbSet<MappingEntry> MappingEntries { get; set; }
        public DbSet<SecretEntity> Secrets { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DatabaseFacade Database { get; }
    }
}
