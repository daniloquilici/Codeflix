using quilici.Codeflix.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.IntegrationTest.Application.UseCases.Category.Common;
using Xunit;
using DomainEntity = quilici.Codeflix.Domain.Entity;

namespace quilici.Codeflix.IntegrationTest.Application.UseCases.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixturaCollection : ICollectionFixture<ListCategoriesTestFixture> { }

    public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
    {
        public List<DomainEntity.Category> GetExampleCategoriesListWithName(List<string> names) => names.Select(x =>
        {
            var category = GetExampleCategory();
            category.Update(x);
            return category;
        }).ToList();

        public List<DomainEntity.Category> CloneCategoryListOrdered(List<DomainEntity.Category> categoryList, string orderBy, SearchOrder order)
        {
            var listClone = new List<DomainEntity.Category>(categoryList);

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
