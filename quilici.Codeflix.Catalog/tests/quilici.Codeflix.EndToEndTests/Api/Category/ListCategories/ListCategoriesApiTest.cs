using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using quilici.Codeflix.Catalog.Api.ApiModels.Response;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.EndToEndTests.Extensions;
using quilici.Codeflix.Catalog.EndToEndTests.Models;
using System.Net;
using Xunit.Abstractions;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{
    [Collection(nameof(ListCategotiesApiTestFixture))]
    public class ListCategoriesApiTest : IDisposable
    {
        private readonly ListCategotiesApiTestFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public ListCategoriesApiTest(ListCategotiesApiTestFixture fixture, ITestOutputHelper testOutputHelper)
        { 
            _fixture = fixture; 
            _testOutputHelper = testOutputHelper;
        }

        [Fact(DisplayName = nameof(ListCategoriesAndTotalByDefault))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ListCategoriesAndTotalByDefault()
        {
            //arrange
            var defaultPerPage = 15;
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);

            //act
            var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories");

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output!.Data.Should().NotBeNull();
            output.Meta.Should().NotBeNull();
            output.Meta!.Total.Should().Be(exampleCategoriesList.Count);
            output.Meta.CurrentPage.Should().Be(1);
            output.Meta.PerPage.Should().Be(defaultPerPage);
            output.Data.Should().HaveCount(defaultPerPage);

            foreach (CategoryModelOutput outputItem in output.Data!)
            {
                var exampleItem = exampleCategoriesList
                    .FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(
                    exampleItem.CreatedAt.TrimMillisseconds()
                );
            }
        }

        [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        public async Task ItemsEmptyWhenPersistenceEmpty()
        {
            //arrange

            //act
            var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories");

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Data.Should().NotBeNull();
            output.Data!.Count.Should().Be(0);
            output.Meta.Should().NotBeNull();
            output.Meta!.Total.Should().Be(0);
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
            var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Data.Should().NotBeNull();
            output.Meta.Should().NotBeNull();
            output.Meta!.Total.Should().Be(exempleCategoriesList.Count);
            output.Meta.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            output.Data!.Count.Should().Be(input.PerPage);
            foreach (var outputItem in output.Data)
            {
                var exampleItem = exempleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();

                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
            }
        }

        [Theory(DisplayName = nameof(ListPaginated))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task ListPaginated(int quantityCategoriesGenerate, int page, int perPage, int expectedQuantityItems)
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategoriesGenerate);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var input = new ListCategoriesInput(page, perPage);

            //act
            var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Meta.Should().NotBeNull();
            output.Meta!.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            output.Meta.Total.Should().Be(exempleCategoriesList.Count);
            output.Data.Should().HaveCount(expectedQuantityItems);
            foreach (var outputItem in output.Data!)
            {
                var exampleItem = exempleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();

                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
            }
        }

        [Theory(DisplayName = nameof(SearchByText))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData("Action", 1, 5, 1, 1)]
        [InlineData("Horror", 1, 5, 3, 3)]
        [InlineData("Horror", 2, 5, 0, 3)]
        [InlineData("Sci-fi", 1, 5, 4, 4)]
        [InlineData("Sci-fi", 1, 2, 2, 4)]
        [InlineData("Sci-fi", 2, 3, 1, 4)]
        [InlineData("Sci-fi Other", 1, 3, 0, 0)]
        [InlineData("Robots", 1, 5, 2, 2)]
        public async Task SearchByText(string search, int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityItems)
        {
            //arrange
            var categoryNamesList = new List<string>() { "Action", "Horror", "Horror - Robots", "Horror - Based on Real Facts", "Drama", "Sci-fi IA", "Sci-fi Space", "Sci-fi Robots", "Sci-fi Future" };
            var exempleCategoriesList = _fixture.GetExampleCategoriesListWithName(categoryNamesList);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var input = new ListCategoriesInput(page, perPage, search);

            //act
            var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Meta.Should().NotBeNull();
            output.Meta!.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            output.Meta.Total.Should().Be(expectedQuantityItems);
            output.Data.Should().HaveCount(expectedQuantityItemsReturned);
            foreach (var outputItem in output.Data!)
            {
                var exampleItem = exempleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();

                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
            }
        }

        [Theory(DisplayName = nameof(ListOrdered))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("id", "asc")]
        [InlineData("id", "desc")]
        [InlineData("", "asc")]
        public async Task ListOrdered(string orderBy, string order)
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(10);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var input = new ListCategoriesInput(page: 1, perPage: 20, sort: orderBy, dir: inputOrder);

            //act
            var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Meta.Should().NotBeNull();
            output.Meta!.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            output.Meta.Total.Should().Be(exempleCategoriesList.Count);
            output.Data.Should().HaveCount(exempleCategoriesList.Count);

            var expectedOrderedList = _fixture.CloneCategoryListOrdered(exempleCategoriesList, input.Sort, input.Dir);

            var count = 0;
            var expectedArr = expectedOrderedList.Select(x => $"{++count} {x.Name} {x.CreatedAt} {JsonConvert.SerializeObject(x)}");
            count = 0;
            var outputArr = output.Data!.Select(x => $"{++count} {x.Name} {x.CreatedAt} {JsonConvert.SerializeObject(x)}");

            _testOutputHelper.WriteLine("Expecteds...");
            _testOutputHelper.WriteLine(string.Join("\n", expectedArr));
            _testOutputHelper.WriteLine("Outputs...");
            _testOutputHelper.WriteLine(string.Join("\n", outputArr));

            for (int i = 0; i < expectedOrderedList.Count; i++)
            {
                var outputItem = output.Data![i];
                var exampleItem = expectedOrderedList[i];

                outputItem.Should().NotBeNull();
                exampleItem.Should().NotBeNull();

                outputItem.Id.Should().Be(exampleItem.Id);
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
            }
        }

        [Theory(DisplayName = nameof(ListOrderedDates))]
        [Trait("EndToEnd/API", "Category/List - Endpoints")]
        [InlineData("CreatedAt", "asc")]
        [InlineData("CreatedAt", "desc")]
        public async Task ListOrderedDates(string orderBy, string order)
        {
            //arrange
            var exempleCategoriesList = _fixture.GetExampleCategoriesList(10);
            await _fixture.Persistence.InsertList(exempleCategoriesList);
            var inputOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var input = new ListCategoriesInput(page: 1, perPage: 20, sort: orderBy, dir: inputOrder);

            //act
            var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CategoryModelOutput>>("/categories", input);

            //assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Meta.Should().NotBeNull();
            output.Meta!.CurrentPage.Should().Be(input.Page);
            output.Meta.PerPage.Should().Be(input.PerPage);
            output.Meta.Total.Should().Be(exempleCategoriesList.Count);
            output.Data.Should().HaveCount(exempleCategoriesList.Count);
            DateTime? lastItemDate = null;

            foreach (var outputItem in output.Data!)
            {
                var exampleItem = exempleCategoriesList.FirstOrDefault(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();

                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());

                if (lastItemDate != null)
                {
                    if (order == "asc")
                        Assert.True(outputItem.CreatedAt >= lastItemDate);
                    else
                        Assert.True(outputItem.CreatedAt <= lastItemDate);
                }
                lastItemDate = outputItem.CreatedAt;
            }
        }

        public void Dispose()
            => _fixture.CleanPersistence();
    }
}
