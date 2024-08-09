using quilici.Codeflix.IntegrationTest.Application.UseCases.Category.Common;
using Xunit;

namespace quilici.Codeflix.IntegrationTest.Application.UseCases.Category.DeleteCategory
{
    [CollectionDefinition(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTestFixtureCollection : ICollectionFixture<DeleteCategoryTestFixture> { }

    public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
    {
    }
}
