using quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.Common;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.DeleteGenre;

[CollectionDefinition(nameof(DeleteGenreApiTestFixture))]
public class DeleteGenreApiTestFixtureCollection : ICollectionFixture<DeleteGenreApiTestFixture> { }

public class DeleteGenreApiTestFixture : GenreBaseFixture
{
}
