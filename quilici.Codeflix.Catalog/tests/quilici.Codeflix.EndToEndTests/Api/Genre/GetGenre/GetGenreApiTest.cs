using FluentAssertions;
using Microsoft.AspNetCore.Http;
using quilici.Codeflix.Catalog.Api.ApiModels.Response;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using System.Net;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.GetGenre;

[Collection(nameof(GetGenreApiTestFixture))]
public class GetGenreApiTest
{
    private readonly GetGenreApiTestFixture _fixture;

    public GetGenreApiTest(GetGenreApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetGenre))]
    [Trait("EndToEnd/API", "Genre/Get - Endpoints")]
    public async Task GetGenre() 
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        var targetGenre = exampleGenres[5];
        await _fixture.Persistence.InsertList(exampleGenres);

        var (response, output) = await _fixture.ApiClient.Get<ApiResponse<GenreModelOuput>>($"/genres/{targetGenre.Id}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Id.Should().Be(targetGenre.Id);
        output.Data.Name.Should().Be(targetGenre.Name);
        output.Data.IsActive.Should().Be(targetGenre.IsActive);
    }
}
