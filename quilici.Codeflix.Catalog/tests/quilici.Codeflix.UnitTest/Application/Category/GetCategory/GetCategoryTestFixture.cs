using quilici.Codeflix.Catalog.UnitTest.Application.Category.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Category.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }

    public class GetCategoryTestFixture : CategoryUsesCaseBaseFixture
    {
    }
}
