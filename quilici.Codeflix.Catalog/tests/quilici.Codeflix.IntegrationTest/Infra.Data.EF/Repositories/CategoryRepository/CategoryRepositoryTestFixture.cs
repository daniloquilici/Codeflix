using quilici.Codeflix.Domain.Entity;
using quilici.Codeflix.Domain.SeedWork.SearchableRepository;
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
        public List<Category> GetExampleCategoriesListWithName(List<string> names) => names.Select(x =>
            {
                var category = GetExampleCategory();
                category.Update(x);
                return category;
            }).ToList();

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
