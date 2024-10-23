using FluentAssertions;
using Microsoft.AspNetCore.Http;
using quilici.Codeflix.Catalog.Api.ApiModels.Response;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.CreateGenre;

[Collection(nameof(CreateGenreApiTestFixture))]
public class CreateGenreApiTest
{
    private readonly CreateGenreApiTestFixture _fixture;

    public CreateGenreApiTest(CreateGenreApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateGenre))]
    [Trait("EndToEnd/Api", "Genre/CreateGenre - Endpoints")]
    public async Task CreateGenre()
    {
        var input = new CreateGenreInput(_fixture.GetValidGenreName(), _fixture.GetRandoBoolean());

        var (response, output) = await _fixture.ApiClient.Post<ApiResponse<GenreModelOutput>>("/genres", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);
        output.Should().NotBeNull();
        output!.Data.Should().NotBeNull();
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(input.Name);
        output.Data.IsActive.Should().Be(input.IsActive);
        output.Data.Categories.Should().HaveCount(0);

        var genreFromDb = await _fixture.Persistence.GetById(output.Data.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Name.Should().Be(input.Name);
        genreFromDb!.IsActive.Should().Be(input.IsActive);
    }
}
