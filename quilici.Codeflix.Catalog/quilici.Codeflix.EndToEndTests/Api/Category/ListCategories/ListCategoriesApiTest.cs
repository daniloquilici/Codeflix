using FluentAssertions;
using Microsoft.AspNetCore.Http;
using quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{
    [Collection(nameof(ListCategotiesApiTestFixture))]
    public class ListCategoriesApiTest : IDisposable
    {
        private readonly ListCategotiesApiTestFixture _fixture;

        public ListCategoriesApiTest(ListCategotiesApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ListCategoriesAndTotalByDefault()
        {
            //arrange
            var defaultPerPage = 15;
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);

            //act
            var (response, output) = await _fixture.ApiClient.Get<ListCategoriesOutput>("/categories");

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Total.Should().Be(exempleCategoriesList.Count);
            output.Page.Should().Be(1);
            output.PerPage.Should().Be(defaultPerPage);
            output.Items.Should().NotBeNull();
            output.Items.Count.Should().Be(defaultPerPage);
            foreach (var outputItem in output.Items)
            {
                var exampleItem = exempleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();

                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ItemsEmptyWhenPersistenceEmpty()
        {
            //arrange

            //act
            var (response, output) = await _fixture.ApiClient.Get<ListCategoriesOutput>("/categories");

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Total.Should().Be(0);
            output.Items.Should().NotBeNull();
            output.Items.Count.Should().Be(0);
        }

        [Fact(DisplayName = nameof(ListCategoriesAndTotal))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ListCategoriesAndTotal()
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var input = new ListCategoriesInput(1, 5);

            //act
            var (response, output) = await _fixture.ApiClient.Get<ListCategoriesOutput>("/categories", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Total.Should().Be(exempleCategoriesList.Count);
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Items.Should().NotBeNull();
            output.Items.Count.Should().Be(input.PerPage);
            foreach (var outputItem in output.Items)
            {
                var exampleItem = exempleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();

                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        public void Dispose()
            => _fixture.CleanPersistence();
    }
}
