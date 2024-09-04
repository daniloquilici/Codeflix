using quilici.Codeflix.Catalog.UnitTest.Common;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.Entity.Genre
{
    [CollectionDefinition(nameof(GenreTestFixture))]
    public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture> { }

    public class GenreTestFixture : BaseFixture
    {
        public string GetValidName()
            => Faker.Commerce.Categories(1)[0];

        public DomainEntity.Genre GetExampleGenre(bool isActive = true) 
            => new DomainEntity.Genre(GetValidName(), isActive);

    }
}
