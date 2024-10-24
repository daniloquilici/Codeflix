﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quilici.Codeflix.Catalog.Api.ApiModels.Response;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using quilici.Codeflix.Catalog.EndToEndTests.Extensions;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory
{
    [Collection(nameof(GetCategoryApiTestFixture))]
    public class GetCategoryApiTest : IDisposable
    {
        private readonly GetCategoryApiTestFixture _fixture;

        public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task GetCategory()
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var exampleCategory = exempleCategoriesList[10];

            //act
            var (response, output) = await _fixture.ApiClient.Get<ApiResponse<CategoryModelOutput>>($"/categories/{exampleCategory.Id}");

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output!.Data.Should().NotBeNull();
            output.Data.Id.Should().Be(exampleCategory.Id);
            output.Data.Name.Should().Be(exampleCategory.Name);
            output.Data.Description.Should().Be(exampleCategory.Description);
            output.Data.IsActive.Should().Be(exampleCategory.IsActive);
            output.Data.CreatedAt.TrimMillisseconds().Should().Be(exampleCategory.CreatedAt.TrimMillisseconds());
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task ErrorWhenNotFound()
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var randowGuid = Guid.NewGuid();

            //act
            var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>($"/categories/{randowGuid}");

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Title.Should().Be("Not Found");
            output!.Type.Should().Be("NotFound");
            output.Detail.Should().Be($"Category '{randowGuid}' not found.");
        }
        public void Dispose()
            => _fixture.CleanPersistence();
    }
}
