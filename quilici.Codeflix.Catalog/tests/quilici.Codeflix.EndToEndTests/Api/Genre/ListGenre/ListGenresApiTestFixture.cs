using quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.Common;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.ListGenre;

[CollectionDefinition(nameof(ListGenresApiTestFixture))]
public class ListGenresApiTestFixtureCollection : ICollectionFixture<ListGenresApiTestFixture> { }

public class ListGenresApiTestFixture : GenreBaseFixture
{
}
