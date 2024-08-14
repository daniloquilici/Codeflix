using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Category.ListCategories
{
    [Collection(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTest
    {
        private readonly ListCategoriesTestFixture _fixture;

        public ListCategoriesTest(ListCategoriesTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(List))]
        [Trait("Appliacation", "ListCategories - Use Cases")]
        public async Task List()
        {
            //Arrange
            var categoriesExampleList = _fixture.GetExampleCategoryList();
            var repositoryMock = _fixture.GetCategoryRepositoryMock();
            var input = _fixture.GetExampleInput();

            var outputRepositorySearch = new SearchOutput<Catalog.Domain.Entity.Category>(
                        currentPage: input.Page,
                        perPage: input.PerPage,
                        items: _fixture.GetExampleCategoryList(),
                        total: new Random().Next(50, 200));

            repositoryMock.Setup(x => x.Search(It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ), It.IsAny<CancellationToken>())).ReturnsAsync(outputRepositorySearch);

            var useCase = new UseCase.ListCategories(repositoryMock.Object);

            //Act
            var output = await useCase.Handle(input, CancellationToken.None);

            //Assert
            output.Should().NotBeNull();
            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
            ((List<CategoryModelOutput>)output.Items).ForEach(outpuItem =>
            {
                var repositoryCategory = outputRepositorySearch.Items.FirstOrDefault(y => y.Id == outpuItem.Id);
                outpuItem.Should().NotBeNull();
                outpuItem.Name.Should().Be(repositoryCategory!.Name);
                outpuItem.Description.Should().Be(repositoryCategory.Description);
                outpuItem.IsActive.Should().Be(repositoryCategory.IsActive);
                outpuItem.Id.Should().Be(repositoryCategory.Id);
                outpuItem.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
            });

            repositoryMock.Verify(x => x.Search(It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
        [Trait("Appliacation", "ListCategories - Use Cases")]
        [MemberData(nameof(ListCategoriesTestDataGenerator.GetinputsWithoutAllParameters), parameters: 14, MemberType = typeof(ListCategoriesTestDataGenerator))]
        public async Task ListInputWithoutAllParameters(ListCategoriesInput input)
        {
            //Arrange
            var categoriesExampleList = _fixture.GetExampleCategoryList();
            var repositoryMock = _fixture.GetCategoryRepositoryMock();
            var outputRepositorySearch = new SearchOutput<Catalog.Domain.Entity.Category>(
                        currentPage: input.Page,
                        perPage: input.PerPage,
                        items: _fixture.GetExampleCategoryList(),
                        total: new Random().Next(50, 200));

            repositoryMock.Setup(x => x.Search(It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ), It.IsAny<CancellationToken>())).ReturnsAsync(outputRepositorySearch);

            var useCase = new UseCase.ListCategories(repositoryMock.Object);

            //Act
            var output = await useCase.Handle(input, CancellationToken.None);

            //Assert
            output.Should().NotBeNull();
            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
            ((List<CategoryModelOutput>)output.Items).ForEach(outpuItem =>
            {
                var repositoryCategory = outputRepositorySearch.Items.FirstOrDefault(y => y.Id == outpuItem.Id);
                outpuItem.Should().NotBeNull();
                outpuItem.Name.Should().Be(repositoryCategory!.Name);
                outpuItem.Description.Should().Be(repositoryCategory.Description);
                outpuItem.IsActive.Should().Be(repositoryCategory.IsActive);
                outpuItem.Id.Should().Be(repositoryCategory.Id);
                outpuItem.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
            });

            repositoryMock.Verify(x => x.Search(It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = nameof(ListOkWhenEmpty))]
        [Trait("Appliacation", "ListCategories - Use Cases")]
        public async Task ListOkWhenEmpty()
        {
            //Arrange
            var repositoryMock = _fixture.GetCategoryRepositoryMock();
            var input = _fixture.GetExampleInput();

            var outputRepositorySearch = new SearchOutput<Catalog.Domain.Entity.Category>(
                        currentPage: input.Page,
                        perPage: input.PerPage,
                        items: new List<Catalog.Domain.Entity.Category>().AsReadOnly(),
                        total: 0);

            repositoryMock.Setup(x => x.Search(It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ), It.IsAny<CancellationToken>())).ReturnsAsync(outputRepositorySearch);

            var useCase = new UseCase.ListCategories(repositoryMock.Object);

            //Act
            var output = await useCase.Handle(input, CancellationToken.None);

            //Assert
            output.Should().NotBeNull();
            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);

            repositoryMock.Verify(x => x.Search(It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
