using quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.CreateGenre;

[CollectionDefinition(nameof(CreateGenreTestFixture))]
public class CreateGenreTestFixtureCollection : ICollectionFixture<CreateGenreTestFixture> { }

public class CreateGenreTestFixture : GenreUseCasesBaseFixture
{
    public CreateGenreInput GetExampleInput() => new CreateGenreInput(GetValidGenreName(), GetRandoBoolean());
}
