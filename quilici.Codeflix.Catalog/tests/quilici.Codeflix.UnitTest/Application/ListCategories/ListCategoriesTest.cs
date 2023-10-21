using Moq;
using quilici.Codeflix.Domain.Entity;
using Xunit;
using FluentAssertions;
using UseCase = quilici.Codeflix.Application.UseCases.Category.ListCategories;
using quilici.Codeflix.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Application.UseCases.Category.Common;

namespace quilici.Codeflix.UnitTest.Application.ListCategories
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
            var input = new UseCase.ListCategoriesInput(
                page: 2,
                perPage: 15,
                search: "search-example",
                sort: "name",
                dir: SearchOrder.Asc
                );

            var outputRepositorySearch = new SearchOutput<Category>(
                        currentPage: input.Page,
                        perPage: input.PerPage,
                        items: (IReadOnlyList<Category>)_fixture.GetExampleCategoryList(),
                        total: 70);

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
                outpuItem.CreatedAt.Should().Be(repositoryCategory.CreateAt);
            });

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
