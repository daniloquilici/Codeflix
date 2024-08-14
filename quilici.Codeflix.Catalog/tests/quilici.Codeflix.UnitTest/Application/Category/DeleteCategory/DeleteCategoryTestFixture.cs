using quilici.Codeflix.Catalog.UnitTest.Application.Category.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Category.DeleteCategory
{
    [CollectionDefinition(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture>
    {
    }

    public class DeleteCategoryTestFixture : CategoryUsesCaseBaseFixture
    {
    }
}
