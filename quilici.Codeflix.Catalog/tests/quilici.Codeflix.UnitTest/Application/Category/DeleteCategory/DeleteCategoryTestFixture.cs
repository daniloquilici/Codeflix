using quilici.Codeflix.UnitTest.Application.Category.Common;
using Xunit;

namespace quilici.Codeflix.UnitTest.Application.Category.DeleteCategory
{
    [CollectionDefinition(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture>
    {
    }

    public class DeleteCategoryTestFixture : CategoryUsesCaseBaseFixture
    {
    }
}
