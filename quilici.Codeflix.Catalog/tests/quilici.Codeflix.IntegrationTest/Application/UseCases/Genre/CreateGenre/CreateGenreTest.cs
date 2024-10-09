using FluentAssertions;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;


namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.CreateGenre;

[Collection(nameof(CreateGenreTestFixture))]
public class CreateGenreTest
{
    private readonly CreateGenreTestFixture _fixture;

    public CreateGenreTest(CreateGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateGenre))]
    [Trait("Integratrion/Applicatrion", "CreateGenre - Use cases")]
    public async Task CreateGenre() 
    {
        CreateGenreInput input = _fixture.GetExampleInput();
        var actDbContext = _fixture.CreateDbContext();
        UseCase.CreateGenre createGente = new UseCase.CreateGenre(new GenreRepository(actDbContext), new UnitOfWork(actDbContext), new CategoryRepository(actDbContext));
        var output = await createGente.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Categories.Should().HaveCount(0);

        var assertDbContext = _fixture.CreateDbContext(true);
        var genreFromDb = await assertDbContext.Genres.FindAsync(output.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Name.Should().Be(input.Name);
        genreFromDb.IsActive.Should().Be(input.IsActive);
    }
}
