using quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.UpdateGenre;

[CollectionDefinition(nameof(UpdateGenreTestFixture))]
public class UpdateGenreTestFixtureCollection : ICollectionFixture<UpdateGenreTestFixture> { }

public class UpdateGenreTestFixture : GenreUseCasesBaseFixture
{
}
