using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryAlias> CategoryAliases { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Mapping> Mappings { get; set; }
        public DbSet<MappingEntry> MappingEntries { get; set; }
        public DbSet<SecretEntity> Secrets { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); // dla sql lite na floata zmienić

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId);

            modelBuilder.Entity<Mapping>()
                .HasMany(m => m.MappingEntries)
                .WithOne(me => me.Mapping)
                .HasForeignKey(me => me.MappingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Aliases)
                .WithOne(ca => ca.Category)
                .HasForeignKey(ca => ca.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SecretEntity>(e =>
            {
                e.HasIndex(e => e.Name).IsUnique();
                e.Property(e => e.Value).IsRequired();
                e.Property(e => e.Name).IsRequired();
            });
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
