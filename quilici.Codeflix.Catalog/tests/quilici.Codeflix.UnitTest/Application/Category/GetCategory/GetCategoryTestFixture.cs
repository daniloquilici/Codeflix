using quilici.Codeflix.UnitTest.Application.Category.Common;
using Xunit;

namespace quilici.Codeflix.UnitTest.Application.Category.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }

    public class GetCategoryTestFixture : CategoryUsesCaseBaseFixture
    {
    }
}
