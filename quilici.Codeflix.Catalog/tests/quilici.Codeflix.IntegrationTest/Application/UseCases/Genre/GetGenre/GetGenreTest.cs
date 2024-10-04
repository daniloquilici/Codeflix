using quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.Common;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.GetGenre;
internal class GetGenreTest : GenreUseCasesBaseFixture
{
    private readonly GetGenreTestFixture _fixture;

    public GetGenreTest(GetGenreTestFixture fixture)
    {
        _fixture = fixture;
    }
}
