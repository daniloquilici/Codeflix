using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Domain.Entity;
using quilici.Codeflix.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Infra.Data.EF;
using quilici.Codeflix.IntegrationTest.Base;
using Xunit;

namespace quilici.Codeflix.IntegrationTest.Infra.Data.EF.Repositories.CategoryRepository
{
    [CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture>
    {
    }
    public class CategoryRepositoryTestFixture : BaseFixture
    {
        public string GetValidCategoryName()
        {
            var categoryName = string.Empty;

            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];

            if (categoryName.Length > 255)
                categoryName = categoryName[..255];

            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            var categoryDescription = Faker.Commerce.ProductDescription();

            if (categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription[..10_000];

            return categoryDescription;
        }

        public bool GetRandoBoolean() => new Random().NextDouble() < 0.5;

        public Category GetExampleCategory() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());

        public List<Category> GetExampleCategoriesList(int length = 10) => Enumerable.Range(0, length)
            .Select(_ => GetExampleCategory()).ToList();

        public List<Category> GetExampleCategoriesListWithName(List<string> names) => names.Select(x =>
            {
                var category = GetExampleCategory();
                category.Update(x);
                return category;
            }).ToList();
            
        public CodeFlixCatalogDbContext CreateDbContext(bool preserveData = false)
        {            
            var context = new CodeFlixCatalogDbContext(new DbContextOptionsBuilder<CodeFlixCatalogDbContext>().UseInMemoryDatabase("integration-tests-db")
                .Options);

            if (preserveData == false)
                context.Database.EnsureDeleted();

            return context;
        }

        public List<Category> CloneCategoryListOrdered(List<Category> categoryList, string orderBy, SearchOrder order) 
        {
            var listClone = new List<Category>(categoryList);

            var orderedEnumerable = (orderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
                ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
                _ => listClone.OrderBy(x => x.Name),
            };

            return orderedEnumerable.ToList();
        }
    }
}
