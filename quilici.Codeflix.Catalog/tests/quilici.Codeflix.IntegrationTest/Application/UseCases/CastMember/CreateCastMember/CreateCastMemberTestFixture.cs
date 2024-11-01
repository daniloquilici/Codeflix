using quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.CreateCastMember;

[CollectionDefinition(nameof(CreateCastMemberTestFixture))]
public class CreateCastMemberTestFixtureCollection : ICollectionFixture<CreateCastMemberTestFixture> { }

public class CreateCastMemberTestFixture : CastMemberUseCasesBaseFixture
{
}
