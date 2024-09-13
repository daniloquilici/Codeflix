using quilici.Codeflix.Catalog.UnitTest.Application.Genre.Commom;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.UpdateGenre
{
    [CollectionDefinition(nameof(UpdateGenreTestFixture))]
    public class UpdateGenreTestFixtureCollection : ICollectionFixture<UpdateGenreTestFixture> { }


    public class UpdateGenreTestFixture : GenreUsesCaseBaseFixture
    {
    }
}
