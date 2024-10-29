using quilici.Codeflix.Catalog.UnitTest.Application.CastMember.Common;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Application.CastMember.ListCastMember;

[CollectionDefinition(nameof(ListCastMemberTestFixture))]
public class ListCastMemberTestFixtureCollection : ICollectionFixture<ListCastMemberTestFixture> { }
public class ListCastMemberTestFixture : CastMemberUsesCasesBaseFixture
{
    public List<DomainEntity.CastMember> GetExampleCastMembersList(int quantity)
        => Enumerable.Range(1, quantity).Select(_ => GetExampleCastMember()).ToList();
}
