using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Domain.Entity;
using quilici.Codeflix.Catalog.Infra.Data.EF.Configurations;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;

namespace quilici.Codeflix.Catalog.Infra.Data.EF
{
    public class CodeFlixCatalogDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<GenresCategories> GenresCategories => Set<GenresCategories>();

        public CodeFlixCatalogDbContext(DbContextOptions<CodeFlixCatalogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());

            modelBuilder.ApplyConfiguration(new GenresCategoriesConfiguration());
        }
    }
}
