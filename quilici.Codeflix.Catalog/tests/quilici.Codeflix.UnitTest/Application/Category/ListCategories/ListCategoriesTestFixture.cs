using quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.UnitTest.Application.Category.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture>
    {
    }

    public class ListCategoriesTestFixture : CategoryUsesCaseBaseFixture
    {
        public List<Catalog.Domain.Entity.Category> GetExampleCategoryList(int length = 10)
        {
            var list = new List<Catalog.Domain.Entity.Category>();
            for (var i = 0; i < length; i++)
                list.Add(GetExampleCategory());

            return list;
        }

        public ListCategoriesInput GetExampleInput()
        {
            var random = new Random();
            return new ListCategoriesInput(
                page: random.Next(1, 10),
                perPage: random.Next(15, 100),
                search: Faker.Commerce.ProductName(),
                sort: Faker.Commerce.ProductName(),
                dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
                );
        }
    }
}
