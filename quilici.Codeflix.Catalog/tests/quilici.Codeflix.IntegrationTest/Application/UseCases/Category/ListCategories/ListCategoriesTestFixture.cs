using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixturaCollection : ICollectionFixture<ListCategoriesTestFixture> { }

    public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
    {
        public List<Domain.Entity.Category> GetExampleCategoriesListWithName(List<string> names) => names.Select(x =>
        {
            var category = GetExampleCategory();
            category.Update(x);
            return category;
        }).ToList();

        public List<Domain.Entity.Category> CloneCategoryListOrdered(List<Domain.Entity.Category> categoryList, string orderBy, SearchOrder order)
        {
            var listClone = new List<Domain.Entity.Category>(categoryList);

            var orderedEnumerable = (orderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
                ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
                ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
                _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
            };

            return orderedEnumerable.ToList();
        }
    }
}
