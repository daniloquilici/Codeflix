using FluentAssertions;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using System.Runtime.InteropServices;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.ListGenres;

[Collection(nameof(ListGenresTestFixture))]
public class ListGenresTest
{
    private readonly ListGenresTestFixture _fixture;

    public ListGenresTest(ListGenresTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListGenres))]
    [Trait("Integration/Application", "ListGenres - UseCases")]
    public async void ListGenres()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre();
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.Genres.AddRangeAsync(exampleGenres);
        await arrangeDbContext.SaveChangesAsync();

        var useCase = new UseCase.ListGenres(new GenreRepository(_fixture.CreateDbContext(true)));
        var input = new UseCase.ListGenresInput(1, 20);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleGenres.Count);
        output.Items.Should().HaveCount(exampleGenres.Count);
        output.Items.ToList().ForEach(outputItem => 
        {
            DomainEntity.Genre? exampleItem = exampleGenres.Find(example => example.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        });
    }

    [Fact(DisplayName = nameof(ListGenresReturnsEmptyWhenPersistenceIsEmpty))]
    [Trait("Integration/Application", "ListGenres - UseCases")]
    public async void ListGenresReturnsEmptyWhenPersistenceIsEmpty()
    {
        var useCase = new UseCase.ListGenres(new GenreRepository(_fixture.CreateDbContext()));
        var input = new UseCase.ListGenresInput(1, 20);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }
}
