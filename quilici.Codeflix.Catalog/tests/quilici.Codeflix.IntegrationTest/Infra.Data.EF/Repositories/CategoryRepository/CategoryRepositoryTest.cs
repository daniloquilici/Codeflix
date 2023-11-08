using FluentAssertions;
using quilici.Codeflix.Application.Exceptions;
using quilici.Codeflix.Domain.Entity;
using quilici.Codeflix.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Infra.Data.EF;
using Xunit;
using Repository = quilici.Codeflix.Infra.Data.EF.Repositories;

namespace quilici.Codeflix.IntegrationTest.Infra.Data.EF.Repositories.CategoryRepository
{
    [Collection(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTest
    {
        private CategoryRepositoryTestFixture _fixture;

        public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(Insert))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Insert()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            await categoryRepository.Insert(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(Get))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Get()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var exampleCategoryList = _fixture.GetExampleCategoriesList(15);
            exampleCategoryList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));

            var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

            dbCategory.Should().NotBeNull();
            dbCategory.Id.Should().Be(exampleCategory.Id);
            dbCategory.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(GetThrowIfNotFound))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task GetThrowIfNotFound()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleId = Guid.NewGuid();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList(15));
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var categoryRepository = new Repository.CategoryRepository(dbContext);

            var task = async () => await categoryRepository.Get(exampleId, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{exampleId}' not found.");
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Update()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var newCategoryValues = _fixture.GetExampleCategory();
            var exampleCategoryList = _fixture.GetExampleCategoriesList(15);
            exampleCategoryList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
            await categoryRepository.Update(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
            //var dbCategory = await dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Id.Should().Be(exampleCategory.Id);
            dbCategory.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(Delete))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Delete()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var exampleCategoryList = _fixture.GetExampleCategoriesList(15);
            exampleCategoryList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            await categoryRepository.Delete(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync();

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
            //var dbCategory = await dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == exampleCategory.Id);

            dbCategory.Should().BeNull();
        }

        [Fact(DisplayName = nameof(SeachReturnsListAndTotal))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SeachReturnsListAndTotal()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoryList = _fixture.GetExampleCategoriesList(15);
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var seachInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(seachInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(seachInput.Page);
            output.PerPage.Should().Be(seachInput.PerPage);
            output.Total.Should().Be(exampleCategoryList.Count);
            output.Items.Should().HaveCount(exampleCategoryList.Count);
            foreach (Category outputItem in output.Items)
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

        [Fact(DisplayName = nameof(SeachReturnsEmptyWhenPersistenceEmpty))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SeachReturnsEmptyWhenPersistenceEmpty()
        {
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var seachInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(seachInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(seachInput.Page);
            output.PerPage.Should().Be(seachInput.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = nameof(SeachReturnsPaginated))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
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
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var seachInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(seachInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(seachInput.Page);
            output.PerPage.Should().Be(seachInput.PerPage);
            output.Total.Should().Be(quantityCategoriesGenerate);
            output.Items.Should().HaveCount(expectedQuantityItems);
            foreach (Category outputItem in output.Items)
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
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
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
            CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoryList = _fixture.GetExampleCategoriesListWithName(new List<string>() { "Action", "Horror", "Horror - Robots", "Horror - Based on Real Facts", "Drama", "Sci-fi IA", "Sci-fi Space", "Sci-fi Robots", "Sci-fi Future" });
            await dbContext.AddRangeAsync(exampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var seachInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

            var output = await categoryRepository.Search(seachInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(seachInput.Page);
            output.PerPage.Should().Be(seachInput.PerPage);
            output.Total.Should().Be(expectedQuantityItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);
            foreach (Category outputItem in output.Items)
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
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
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
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var seachInput = new SearchInput(1, 20, "", orderBy, searchOrder);

            var output = await categoryRepository.Search(seachInput, CancellationToken.None);

            var expectedOrderedList = _fixture.CloneCategoryListOrdered(exampleCategoryList, orderBy, searchOrder);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(seachInput.Page);
            output.PerPage.Should().Be(seachInput.PerPage);
            output.Total.Should().Be(exampleCategoryList.Count);
            output.Items.Should().HaveCount(exampleCategoryList.Count);
            for (int i = 0; i < expectedOrderedList.Count; i++)
            {
                var expectedItem = expectedOrderedList[i];
                var outputItem = output.Items[i];

                expectedItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Id.Should().Be(expectedItem.Id);
                outputItem.Name.Should().Be(expectedItem.Name);
                outputItem.Description.Should().Be(expectedItem.Description);
                outputItem.IsActive.Should().Be(expectedItem.IsActive);
                outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
            }
        }
    }
}
