﻿using FluentAssertions;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using ApplicationUseCases = quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.ListCategories
{
    [Collection(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTest
    {
        private readonly ListCategoriesTestFixture _fixture;

        public ListCategoriesTest(ListCategoriesTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(SeachReturnsListAndTotal))]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
        public async Task SeachReturnsListAndTotal()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new CategoryRepository(dbContext);
            var input = new ListCategoriesInput(1, 20);
            var useCase = new ApplicationUseCases.ListCategories(categoryRepository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(exampleCategoryList.Count);
            output.Items.Should().HaveCount(exampleCategoryList.Count);
            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoryList.Find(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = nameof(SeachReturnsEmptyWhenEmpty))]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
        public async Task SeachReturnsEmptyWhenEmpty()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var categoryRepository = new CategoryRepository(dbContext);
            var input = new ListCategoriesInput(1, 20);
            var useCase = new ApplicationUseCases.ListCategories(categoryRepository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = nameof(SeachReturnsPaginated))]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task SeachReturnsPaginated(int quantityCategoriesGenerate, int page, int perPage, int expectedQuantityItems)
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoryList = _fixture.GetExampleCategoriesList(quantityCategoriesGenerate);
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new CategoryRepository(dbContext);
            var input = new ListCategoriesInput(page, perPage);
            var useCase = new ApplicationUseCases.ListCategories(categoryRepository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(exampleCategoryList.Count);
            output.Items.Should().HaveCount(expectedQuantityItems);
            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoryList.Find(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Theory(DisplayName = nameof(SearchByText))]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
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
            var categoryNamesList = new List<string>() { "Action", "Horror", "Horror - Robots", "Horror - Based on Real Facts", "Drama", "Sci-fi IA", "Sci-fi Space", "Sci-fi Robots", "Sci-fi Future" };
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoryList = _fixture.GetExampleCategoriesListWithName(categoryNamesList);
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new CategoryRepository(dbContext);
            var input = new ListCategoriesInput(page, perPage, search);
            var useCase = new ApplicationUseCases.ListCategories(categoryRepository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(expectedQuantityItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);
            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoryList.Find(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Theory(DisplayName = nameof(SearchOrdened))]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("id", "asc")]
        [InlineData("id", "desc")]
        [InlineData("CreatedAt", "asc")]
        [InlineData("CreatedAt", "desc")]
        [InlineData("", "asc")]
        public async Task SearchOrdened(string orderBy, string order)
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoryList = _fixture.GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new CategoryRepository(dbContext);
            var useCaseOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var input = new ListCategoriesInput(1, 20, "", orderBy, useCaseOrder);
            var useCase = new ApplicationUseCases.ListCategories(categoryRepository);

            var output = await useCase.Handle(input, CancellationToken.None);

            var expectedOrderedList = _fixture.CloneCategoryListOrdered(exampleCategoryList, input.Sort, input.Dir);
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(exampleCategoryList.Count);
            output.Items.Should().HaveCount(exampleCategoryList.Count);
            for (int i = 0; i < expectedOrderedList.Count; i++)
            {
                var outputItem = output.Items[i];
                var exampleItem = expectedOrderedList[i];

                outputItem.Should().NotBeNull();
                exampleItem.Should().NotBeNull();

                outputItem.Id.Should().Be(exampleItem.Id);
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }
    }
}