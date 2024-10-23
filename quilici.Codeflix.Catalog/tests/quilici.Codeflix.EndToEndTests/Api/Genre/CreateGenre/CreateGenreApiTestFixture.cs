using quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.Common;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreApiTestFixture))]
public class CreateGenreApiTestFixtureCollection : ICollectionFixture<CreateGenreApiTestFixture> { }

public class CreateGenreApiTestFixture : GenreBaseFixture
{
}
