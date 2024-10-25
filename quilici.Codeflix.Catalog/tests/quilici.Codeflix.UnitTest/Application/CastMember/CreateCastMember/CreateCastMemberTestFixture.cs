using quilici.Codeflix.Catalog.UnitTest.Application.CastMember.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.CastMember.CreateCastMember;

[CollectionDefinition(nameof(CreateCastMemberTestFixture))]
public class CreateCastMemberTestFixtureCollection : ICollectionFixture<CreateCastMemberTestFixture> { }

public class CreateCastMemberTestFixture : CastMemberUsesCasesBaseFixture
{

}
