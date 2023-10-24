using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Domain.Entity;
using quilici.Codeflix.Infra.Data.EF.Configurations;

namespace quilici.Codeflix.Infra.Data.EF
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
