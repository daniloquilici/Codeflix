using FluentAssertions;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.GetGenre;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.GetGenre;

[Collection(nameof(GetGenreTestFixture))]
public class GetGenreTest
{
    private readonly GetGenreTestFixture _fixture;

    public GetGenreTest(GetGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetGenre))]
    [Trait("Integratrion/Applicatrion", "GetGenre - Use cases")]
    public async Task GetGenre()
    {
        var genreExampleList = _fixture.GetExampleListGenre(10);
        var expectedGenre = genreExampleList[5];
        var dbArrangeContext = _fixture.CreateDbContext();
        await dbArrangeContext.Genres.AddRangeAsync(genreExampleList);
        await dbArrangeContext.SaveChangesAsync();
        var genreRepository = new GenreRepository(_fixture.CreateDbContext(true));
        var useCase = new UseCase.GetGenre(genreRepository);
        var input = new UseCase.GetGenreInput(expectedGenre.Id);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(expectedGenre.Id);
        output.Name.Should().Be(expectedGenre.Name);
        output.IsActive.Should().Be(expectedGenre.IsActive);
        output.CreatedAt.Should().Be(expectedGenre.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetGenreThrowsWhenNotFound))]
    [Trait("Integratrion/Applicatrion", "GetGenre - Use cases")]
    public async Task GetGenreThrowsWhenNotFound()
    {
        var genreExampleList = _fixture.GetExampleListGenre(10);
        var randomGuid = Guid.NewGuid();
        var dbArrangeContext = _fixture.CreateDbContext();
        await dbArrangeContext.Genres.AddRangeAsync(genreExampleList);
        await dbArrangeContext.SaveChangesAsync();
        var genreRepository = new GenreRepository(_fixture.CreateDbContext(true));
        var useCase = new UseCase.GetGenre(genreRepository);
        var input = new UseCase.GetGenreInput(randomGuid);

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{randomGuid}' not found.");
    }

    [Fact(DisplayName = nameof(GetGenreWithCategoryRelations))]
    [Trait("Integratrion/Applicatrion", "GetGenre - Use cases")]
    public async Task GetGenreWithCategoryRelations()
    {
        var genreExampleList = _fixture.GetExampleListGenre(10);
        var categoriesExapleList = _fixture.GetExampleCategoriesList(5);
        var expectedGenre = genreExampleList[5];
        categoriesExapleList.ForEach(category => expectedGenre.AddCategory(category.Id));
        var dbArrangeContext = _fixture.CreateDbContext();
        await dbArrangeContext.Categories.AddRangeAsync(categoriesExapleList);
        await dbArrangeContext.Genres.AddRangeAsync(genreExampleList);
        await dbArrangeContext.GenresCategories.AddRangeAsync(expectedGenre.Categories.Select(categoryId => new GenresCategories(categoryId, expectedGenre.Id)));
        await dbArrangeContext.SaveChangesAsync();
        var genreRepository = new GenreRepository(_fixture.CreateDbContext(true));
        var useCase = new UseCase.GetGenre(genreRepository);
        var input = new UseCase.GetGenreInput(expectedGenre.Id);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(expectedGenre.Id);
        output.Name.Should().Be(expectedGenre.Name);
        output.IsActive.Should().Be(expectedGenre.IsActive);
        output.CreatedAt.Should().Be(expectedGenre.CreatedAt);
        output.Categories.Should().HaveCount(expectedGenre.Categories.Count);
        output.Categories.ToList().ForEach(relationModel =>
        {
            expectedGenre.Categories.Should().Contain(relationModel.Id);
            relationModel.Name.Should().BeNull();
        });
    }
}
