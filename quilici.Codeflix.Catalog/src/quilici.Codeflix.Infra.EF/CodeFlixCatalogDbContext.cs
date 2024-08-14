using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Domain.Entity;
using quilici.Codeflix.Catalog.Infra.Data.EF.Configurations;

namespace quilici.Codeflix.Catalog.Infra.Data.EF
{
    public class CodeFlixCatalogDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();

        public CodeFlixCatalogDbContext(DbContextOptions<CodeFlixCatalogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
