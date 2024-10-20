using quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.Common;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.GetGenre;

[CollectionDefinition(nameof(GetGenreApiTestFixture))]
public class GetGenreApiTestFixtureCollection : ICollectionFixture<GetGenreApiTestFixture> { }
public class GetGenreApiTestFixture : GenreBaseFixture
{
    public GetGenreApiTestFixture()
        : base()
    {

    }
}
