using quilici.Codeflix.Catalog.UnitTest.Common;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.Commom
{
    public class GenreUsesCaseBaseFixture : BaseFixture
    {
        public string GetValidGenreName() => Faker.Commerce.Categories(1)[0];
    }
}
