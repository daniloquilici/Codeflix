using FluentAssertions;
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
}
