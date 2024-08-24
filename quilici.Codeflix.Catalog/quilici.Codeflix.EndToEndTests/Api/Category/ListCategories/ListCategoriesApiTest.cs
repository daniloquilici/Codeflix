using FluentAssertions;
using Microsoft.AspNetCore.Http;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{
    [Collection(nameof(ListCategotiesApiTestFixture))]
    public class ListCategoriesApiTest
    {
        private readonly ListCategotiesApiTestFixture _fixture;

        public ListCategoriesApiTest(ListCategotiesApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
        [Trait("EndToEnd/API", "Category/LIst - Endpoints")]
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
    }
}
