using quilici.Codeflix.Catalog.UnitTest.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.Entity.Genre
{
    [CollectionDefinition(nameof(GenreTestFixture))]
    public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture> { }

    public class GenreTestFixture : BaseFixture
    {
    }
}
