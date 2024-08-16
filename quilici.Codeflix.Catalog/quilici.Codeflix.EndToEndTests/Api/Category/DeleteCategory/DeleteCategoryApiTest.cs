using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.DeleteCategory
{
    [Collection(nameof(DeleteCategoryApiTestFixture))]
    public class DeleteCategoryApiTest
    {
        private readonly DeleteCategoryApiTestFixture _fixture;

        public DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
        public async void DeleteCategory() 
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var exampleCategory = exempleCategoriesList[10];

            //act
            var (response, output) = await _fixture.ApiClient.Delete<object>($"/categories/{exampleCategory.Id}");

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
            output.Should().BeNull();
            var pesistenceCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
            pesistenceCategory.Should().BeNull();
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
        public async void ErrorWhenNotFound()
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var randowGuid = Guid.NewGuid();

            //act
            var (response, output) = await _fixture.ApiClient.Delete<ProblemDetails>($"/categories/{randowGuid}");

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Title.Should().Be("Not Found");
            output.Type.Should().Be("NotFound");
            output.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Detail.Should().Be($"Category '{randowGuid}' not found.");
        }
    }
}
