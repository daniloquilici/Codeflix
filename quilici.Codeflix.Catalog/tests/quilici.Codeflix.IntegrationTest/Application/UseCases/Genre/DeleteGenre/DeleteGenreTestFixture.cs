using quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.DeleteGenre;

[CollectionDefinition(nameof(DeleteGenreTestFixture))]
public class DeleteGenreTestFixtureCollection : ICollectionFixture<DeleteGenreTestFixture> { }
public class DeleteGenreTestFixture : GenreUseCasesBaseFixture
{
}
