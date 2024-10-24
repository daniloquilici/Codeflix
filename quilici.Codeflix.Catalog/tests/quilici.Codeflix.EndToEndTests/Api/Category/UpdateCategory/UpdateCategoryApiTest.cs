﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quilici.Codeflix.Catalog.Api.ApiModels.Category;
using quilici.Codeflix.Catalog.Api.ApiModels.Response;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTest : IDisposable
    {
        private readonly UpdateCategoryApiTestFixture _fixture;

        public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(UpdateCategory))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async void UpdateCategory()
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var exampleCategory = exempleCategoriesList[10];
            var input = _fixture.GetExampleInput();

            //act
            var (response, output) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelOutput>>($"/categories/{exampleCategory.Id}", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output!.Data.Should().NotBeNull();
            output.Data.Id.Should().Be(exampleCategory.Id);
            output.Data.Name.Should().Be(input.Name);
            output.Data.Description.Should().Be(input.Description);
            output.Data.IsActive.Should().Be((bool)input.IsActive!);

            var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)input.IsActive!);
        }

        [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async void UpdateCategoryOnlyName()
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var exampleCategory = exempleCategoriesList[10];
            var input = new UpdateCategoryApiInput(_fixture.GetValidCategoryName());

            //act
            var (response, output) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelOutput>>($"/categories/{exampleCategory.Id}", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output!.Data.Should().NotBeNull();
            output.Data.Id.Should().Be(exampleCategory.Id);
            output.Data.Name.Should().Be(input.Name);
            output.Data.Description.Should().Be(exampleCategory.Description);
            output.Data.IsActive.Should().Be(exampleCategory.IsActive!);

            var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive!);
        }

        [Fact(DisplayName = nameof(UpdateCategoryNameAndDescription))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async void UpdateCategoryNameAndDescription()
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var exampleCategory = exempleCategoriesList[10];
            var input = new UpdateCategoryApiInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

            //act
            var (response, output) = await _fixture.ApiClient.Put<ApiResponse<CategoryModelOutput>>($"/categories/{exampleCategory.Id}", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output!.Data.Should().NotBeNull();
            output.Data.Id.Should().Be(exampleCategory.Id);
            output.Data.Name.Should().Be(input.Name);
            output.Data.Description.Should().Be(input.Description);
            output.Data.IsActive.Should().Be(exampleCategory.IsActive!);

            var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive!);
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async void ErrorWhenNotFound()
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var randomGuid = Guid.NewGuid();
            var input = _fixture.GetExampleInput();

            //act
            var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>($"/categories/{randomGuid}", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Title.Should().Be("Not Found");
            output.Type.Should().Be("NotFound");
            output.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Detail.Should().Be($"Category '{randomGuid}' not found.");
        }

        [Theory(DisplayName = nameof(ErrorWhenCantInstantieteAggregate))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        [MemberData(nameof(UpdateCategoryApiTestDataGenerator.GetInvalidInputs), MemberType = typeof(UpdateCategoryApiTestDataGenerator))]
        public async void ErrorWhenCantInstantieteAggregate(UpdateCategoryApiInput input, string expectedDetail)
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var exampleCategory = exempleCategoriesList[10];

            //act
            var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>($"/categories/{exampleCategory.Id}", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
            output.Should().NotBeNull();
            output!.Title.Should().Be("One or more validation errors ocurred");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be(expectedDetail);
        }

        public void Dispose()
            => _fixture.CleanPersistence();
    }
}
