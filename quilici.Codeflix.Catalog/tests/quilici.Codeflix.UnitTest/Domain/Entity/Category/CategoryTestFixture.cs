using quilici.Codeflix.UnitTest.Common;
using Xunit;
using DomainEntity = quilici.Codeflix.Domain.Entity;

namespace quilici.Codeflix.UnitTest.Domain.Entity.Category
{
    [CollectionDefinition(nameof(CategoryTestFixture))]
    public class CategiryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
    {
    }

    public class CategoryTestFixture : BaseFixture
    {
        public CategoryTestFixture()
            : base() { }

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

        public DomainEntity.Category GetValidCategory() => new(GetValidCategoryName(), GetValidCategoryDescription());
    }
}


