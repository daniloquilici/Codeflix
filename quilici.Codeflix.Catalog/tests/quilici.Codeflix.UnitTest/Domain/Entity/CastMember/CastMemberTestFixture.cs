using quilici.Codeflix.Catalog.UnitTest.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.Entity.CastMember;

[CollectionDefinition(nameof(CastMemberTestFixture))]
public class CastMemberTestFixtureCollection : ICollectionFixture<CastMemberTestFixture> { }

public class CastMemberTestFixture : BaseFixture
{
    public string GetValidName()
        => Faker.Name.FullName();

    public CastMemberType GetRandomCastMemberType()
        => (new Random().Next(1, 2)) as CastMemberType;
}
