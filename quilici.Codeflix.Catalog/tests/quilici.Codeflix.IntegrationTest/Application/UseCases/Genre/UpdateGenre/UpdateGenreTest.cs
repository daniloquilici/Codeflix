using FluentAssertions;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.UpdateGenre;

[Collection(nameof(UpdateGenreTestFixture))]
public class UpdateGenreTest
{
    private readonly UpdateGenreTestFixture _fixture;

    public UpdateGenreTest(UpdateGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateGenre))]
    [Trait("Integratrion/Applicatrion", "UpdateGenre - Use cases")]
    public async Task UpdateGenre()
    {
        IList<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        DomainEntity.Genre targetGenre = exampleGenres[5];
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.AddRangeAsync(exampleGenres);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        UseCase.UpdateGenre updateGenre = new UseCase.UpdateGenre(new UnitOfWork(actDbContext), new GenreRepository(actDbContext), new CategoryRepository(actDbContext));
        UpdateGenreInput input = new UpdateGenreInput(targetGenre.Id, _fixture.GetValidGenreName(), !targetGenre.IsActive);

        GenreModelOuput output = await updateGenre.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(targetGenre.Id);
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be((bool)input.IsActive!);

        var assertDbContext = _fixture.CreateDbContext(true);
        DomainEntity.Genre? genreFromDb = await assertDbContext.Genres.FindAsync(targetGenre.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Id.Should().Be(targetGenre.Id);
        genreFromDb.Name.Should().Be(input.Name);
        genreFromDb.IsActive.Should().Be((bool)input.IsActive!);
    }
}
