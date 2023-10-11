using quilici.Codeflix.UnitTest.Domain.Entity.Category;
using Xunit;
using DomainEntity = quilici.Codeflix.Domain.Entity;

namespace quilici.Codeflix.UnitTest.Domain.Entity.Category
{
    public class CategoryTestFixture
    {
        public DomainEntity.Category GetValidCategory() => new ("Category name", "Category description");
    }
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategiryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
{

}
