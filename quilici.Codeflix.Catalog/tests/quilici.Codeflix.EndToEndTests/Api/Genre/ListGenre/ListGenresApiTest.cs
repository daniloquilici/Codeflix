using FluentAssertions;
using quilici.Codeflix.Catalog.Api.ApiModels.Response;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres;
using quilici.Codeflix.Catalog.EndToEndTests.Extensions;
using quilici.Codeflix.Catalog.EndToEndTests.Models;
using System.Net;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.ListGenre;

[Collection(nameof(ListGenresApiTestFixture))]
public class ListGenresApiTest
{
    private readonly ListGenresApiTestFixture _fixture;

    public ListGenresApiTest(ListGenresApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListGenres))]
    [Trait("EndtoEnd/Api", "Genre/ListGenres - Endpoints")]
    public async Task ListGenres()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        var targetGenre = exampleGenres[5];
        await _fixture.Persistence.InsertList(exampleGenres);

        var input = new ListGenresInput(1, exampleGenres.Count);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<GenreModelOutput>>("/genres", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        output.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Data.Should().NotBeNull();
        output.Meta!.CurrentPage.Should().Be(input.Page);
        output.Meta.PerPage.Should().Be(input.PerPage);
        output.Meta.Total.Should().Be(exampleGenres.Count);
        output.Data!.Count.Should().Be(exampleGenres.Count);
        output.Data.ToList().ForEach(outputItem =>
        {
            var exampleItem = exampleGenres.First(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            exampleItem.Name.Should().Be(outputItem.Name);
            exampleItem.IsActive.Should().Be(outputItem.IsActive);
            exampleItem.CreatedAt.TrimMillisseconds().Should().Be(outputItem.CreatedAt.TrimMillisseconds());
        });
    }
}
