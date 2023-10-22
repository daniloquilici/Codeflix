using quilici.Codeflix.Application.UseCases.Category.ListCategories;
using quilici.Codeflix.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.UnitTest.Application.Category.Common;
using Xunit;
using DomianEntity = quilici.Codeflix.Domain.Entity;

namespace quilici.Codeflix.UnitTest.Application.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture>
    {
    }

    public class ListCategoriesTestFixture : CategoryUsesCaseBaseFixture
    {
        public List<DomianEntity.Category> GetExampleCategoryList(int length = 10)
        {
            var list = new List<Category>();
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
