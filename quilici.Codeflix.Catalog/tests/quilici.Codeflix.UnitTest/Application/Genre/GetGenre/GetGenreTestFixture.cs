using quilici.Codeflix.Catalog.UnitTest.Application.Genre.Commom;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.GetGenre
{
    [CollectionDefinition(nameof(GetGenreTestFixture))]
    public class GetGenreTestFixtureCollection : ICollectionFixture<GetGenreTestFixture> { }
    public class GetGenreTestFixture : GenreUseCaseBaseFixture
    {

    }
}
