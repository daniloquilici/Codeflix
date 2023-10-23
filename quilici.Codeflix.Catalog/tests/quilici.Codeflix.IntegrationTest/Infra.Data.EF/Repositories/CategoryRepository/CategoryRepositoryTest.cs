using Xunit;
using FluentAssertions;

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
            CodeflixCatalogDbContext dbContext = _fixture.GetCreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var categoryRepository = new CategoryRepository(dbContext);

            await categoryRepository.Insert(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var dbCategory = await dbContext.Categories.Find(exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }
    }
}
