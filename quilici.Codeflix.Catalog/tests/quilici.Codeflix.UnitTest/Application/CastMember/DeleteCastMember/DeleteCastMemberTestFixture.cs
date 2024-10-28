using quilici.Codeflix.Catalog.UnitTest.Application.CastMember.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.CastMember.DeleteCastMember;

[CollectionDefinition(nameof(DeleteCastMemberTestFixture))]
public class DeleteCastMemberTestFixtureCollection : ICollectionFixture<DeleteCastMemberTestFixture> { }

public class DeleteCastMemberTestFixture : CastMemberUsesCasesBaseFixture
{
}
