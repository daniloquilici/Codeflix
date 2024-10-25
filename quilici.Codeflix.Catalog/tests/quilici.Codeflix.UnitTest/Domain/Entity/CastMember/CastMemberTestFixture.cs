using quilici.Codeflix.Catalog.Domain.Enum;
using quilici.Codeflix.Catalog.UnitTest.Common;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.Entity.CastMember;

[CollectionDefinition(nameof(CastMemberTestFixture))]
public class CastMemberTestFixtureCollection : ICollectionFixture<CastMemberTestFixture> { }

public class CastMemberTestFixture : BaseFixture
{
    public string GetValidName()
        => Faker.Name.FullName();

    public CastMemberType GetRandomCastMemberType()
        => (CastMemberType)(new Random().Next(1, 2));

    public DomainEntity.CastMember GetExempleCastMember()
         => new DomainEntity.CastMember(GetValidName(), GetRandomCastMemberType());
}
