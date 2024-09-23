using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;

namespace quilici.Codeflix.Catalog.Infra.Data.EF.Configurations
{
    internal class GenresCategoriesConfiguration : IEntityTypeConfiguration<GenresCategories>
    {
        public void Configure(EntityTypeBuilder<GenresCategories> builder)
        {
            builder.HasKey(relation => new { relation.CategoryId, relation.GenreId });
        }
    }
}
