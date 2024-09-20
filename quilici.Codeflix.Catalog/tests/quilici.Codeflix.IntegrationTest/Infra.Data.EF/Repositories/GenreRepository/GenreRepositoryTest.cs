using FluentAssertions;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using Xunit;
using Repository = quilici.Codeflix.Catalog.Infra.Data.EF.Repositories.GenreRepository;

namespace quilici.Codeflix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.GenreRepository;
[Collection(nameof(GenreRepositoryTestFixture))]
public class GenreRepositoryTest
{
    private readonly GenreRepositoryTestFixture _fixture
    public GenreRepositoryTest(GenreRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task Insert()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenre = _fixture.GetExampleGenre();
        var categoriesListExample = _fixture.GetExampleCategoriesList(3);
        categoriesListExample.ForEach(x => exampleGenre.AddCategory(x.Id));
        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        var genreRepository = new Repository.GenreRepository(dbContext);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        await genreRepository.Insert(exampleGenre, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var assertesDBContext = _fixture.CreateDbContext(true);
        var dbCategory = await assertesDBContext.Categories.FindAsync(exampleGenre.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(exampleGenre.Name);
        dbCategory.IsActive.Should().Be(exampleGenre.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        var genreCategoriesRelations = await assertesDBContext.GenreCategories.Where(x => x.Id = exampleGenre.Id).ToList();
        genreCategoriesRelations.Should().HaveCount(categoriesListExample.Count);
        genreCategoriesRelations.ForEach(relation => {
            var expectedCategory = categoriesListExample.FirstOrDefault(x => x.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }
}
