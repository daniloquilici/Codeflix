using Moq;
using quilici.Codeflix.Domain.Entity;
using quilici.Codeflix.Domain.Repository;
using quilici.Codeflix.UnitTest.Common;
using Xunit;

namespace quilici.Codeflix.UnitTest.Application.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture>
    {
    }

    public class ListCategoriesTestFixture : BaseFixture
    {
        public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new Mock<ICategoryRepository>();

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

        public List<Category> GetExampleCategoryList(int length = 10) 
        {
            var list = new List<Category>();
            for (var i = 0; i < length; i++)
                list.Add(GetExampleCategory());

            return list;
        }
    }
}
