using quilici.Codeflix.IntegrationTest.Application.UseCases.Category.Common;
using Xunit;

namespace quilici.Codeflix.IntegrationTest.Application.UseCases.Category.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }

    public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
    {
    }
}
