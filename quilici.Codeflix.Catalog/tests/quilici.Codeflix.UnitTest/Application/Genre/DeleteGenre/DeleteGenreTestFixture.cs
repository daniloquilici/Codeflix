using quilici.Codeflix.Catalog.UnitTest.Application.Genre.Commom;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.DeleteGenre
{
    [CollectionDefinition(nameof(DeleteGenreTestFixture))]
    public class DeleteGenreTestFixtureCollection : ICollectionFixture<DeleteGenreTestFixture> { }
    public class DeleteGenreTestFixture : GenreUsesCaseBaseFixture
    {
    }
}
