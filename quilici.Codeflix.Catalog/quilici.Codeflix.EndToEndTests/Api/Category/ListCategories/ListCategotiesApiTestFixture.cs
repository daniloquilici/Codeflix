using quilici.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategotiesApiTestFixture))]
    public class ListCategotiesApiTestFixtureCollection : ICollectionFixture<ListCategotiesApiTestFixture> { }

    public class ListCategotiesApiTestFixture : CategoryBaseFixture
    {
    }
}
