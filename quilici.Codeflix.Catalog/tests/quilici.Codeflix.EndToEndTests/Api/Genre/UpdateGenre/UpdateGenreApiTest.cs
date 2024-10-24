using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quilici.Codeflix.Catalog.Api.ApiModels.Genre;
using quilici.Codeflix.Catalog.Api.ApiModels.Response;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;
using System.Net;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.UpdateGenre;

[Collection(nameof(UpdateGenreTestFixture))]
public class UpdateGenreApiTest
{
    private readonly UpdateGenreTestFixture _fixture;

    public UpdateGenreApiTest(UpdateGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateGenre))]
    [Trait("EndtoEnd/Api", "Genre/UpdateGenre - Endpoints")]
    public async Task UpdateGenre()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        var targetGenre = exampleGenres[5];
        await _fixture.Persistence.InsertList(exampleGenres);
        var input = new UpdateGenreApiInput(_fixture.GetValidGenreName(), _fixture.GetRandoBoolean());

        var (response, output) = await _fixture.ApiClient.Put<ApiResponse<GenreModelOutput>>($"/genres/{targetGenre.Id}", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Id.Should().Be(targetGenre.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.IsActive.Should().Be((bool)input.IsActive!);

        var genreFromDb = await _fixture.Persistence.GetById(output.Data.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Name.Should().Be(input.Name);
        genreFromDb.IsActive.Should().Be((bool)input.IsActive);
    }

    [Fact(DisplayName = nameof(UpdateGenre))]
    [Trait("EndtoEnd/Api", "Genre/UpdateGenre - Endpoints")]
    public async Task ProblemDetailsWhenNotFound()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        var randomGuid = Guid.NewGuid();
        await _fixture.Persistence.InsertList(exampleGenres);
        var input = new UpdateGenreApiInput(_fixture.GetValidGenreName(), _fixture.GetRandoBoolean());

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>($"/genres/{randomGuid}", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Title.Should().Be("Not Found");
        output.Detail.Should().Be($"Genre '{randomGuid}' not found.");
        output.Type.Should().Be("NotFound");
        output.Status.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact(DisplayName = nameof(UpdateGenreWithRelations))]
    [Trait("EndtoEnd/Api", "Genre/UpdateGenre - Endpoints")]
    public async Task UpdateGenreWithRelations()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        var targetGenre = exampleGenres[5];
        List<DomainEntity.Category> exampleCategories = _fixture.GetExampleCategoriesList(10);
        Random random = new Random();
        exampleGenres.ForEach(genres =>
        {
            int relationsCount = random.Next(2, exampleCategories.Count - 1);
            for (int i = 0; i < relationsCount; i++)
            {
                var selected = exampleCategories[random.Next(0, exampleCategories.Count - 1)];
                if (!genres.Categories.Contains(selected.Id))
                    genres.AddCategory(selected.Id);
            }
        });
        var genresCategories = new List<GenresCategories>();
        exampleGenres.ForEach(genre =>
            genre.Categories.ToList().ForEach(categoryId => genresCategories.Add(new GenresCategories(categoryId, genre.Id)))
            );

        int newRelationsCount = random.Next(2, exampleCategories.Count - 1);
        var newRelatedCategoriesIds = new List<Guid>();
        for (int i = 0; i < newRelationsCount; i++)
        {
            var selected = exampleCategories[random.Next(0, exampleCategories.Count - 1)];
            if (!newRelatedCategoriesIds.Contains(selected.Id))
                newRelatedCategoriesIds.Add(selected.Id);
        }

        await _fixture.Persistence.InsertList(exampleGenres);
        await _fixture.CategoryPersistence.InsertList(exampleCategories);
        await _fixture.Persistence.InsertGenresCategoriesRelationsList(genresCategories);

        var input = new UpdateGenreApiInput(_fixture.GetValidGenreName(), _fixture.GetRandoBoolean(), newRelatedCategoriesIds);

        var (response, output) = await _fixture.ApiClient.Put<ApiResponse<GenreModelOutput>>($"/genres/{targetGenre.Id}", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Id.Should().Be(targetGenre.Id);
        output.Data.Name.Should().Be(input.Name);
        output.Data.IsActive.Should().Be((bool)input.IsActive!);
        var relatedCategoriesIdsFromOutput = output.Data.Categories.Select(x => x.Id).ToList();
        relatedCategoriesIdsFromOutput.Should().NotBeNull();
        relatedCategoriesIdsFromOutput.Should().BeEquivalentTo(newRelatedCategoriesIds);

        var genreFromDb = await _fixture.Persistence.GetById(output.Data.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Name.Should().Be(input.Name);
        genreFromDb.IsActive.Should().Be((bool)input.IsActive);

        var genresCategoriesFromDb = await _fixture.Persistence.GetGenresCategoriesRelationsByGenreId(targetGenre.Id);
        var relatedCategoriesIdsFromDb = genresCategoriesFromDb.Select(x => x.CategoryId).ToList();
        relatedCategoriesIdsFromDb.Should().NotBeNull();
        relatedCategoriesIdsFromDb.Should().BeEquivalentTo(newRelatedCategoriesIds);
    }

    [Fact(DisplayName = nameof(ErrorWhenInvalidRelation))]
    [Trait("EndtoEnd/Api", "Genre/UpdateGenre - Endpoints")]
    public async Task ErrorWhenInvalidRelation()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        var targetGenre = exampleGenres[5];
        await _fixture.Persistence.InsertList(exampleGenres);
        var randomGuid = Guid.NewGuid();
        var input = new UpdateGenreApiInput(_fixture.GetValidGenreName(), _fixture.GetRandoBoolean(), new List<Guid>() { randomGuid });

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>($"/genres/{targetGenre.Id}", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
        output.Should().NotBeNull();
        output!.Type.Should().Be("RelatedAggregate");
        output.Detail.Should().Be($"Related category id(s) not found: {randomGuid}");
    }
}
