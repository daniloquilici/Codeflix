using FluentAssertions;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.Common;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.CastMember.GetCastMember;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.GetCastMember;

[Collection(nameof(CastMemberUseCasesBaseFixture))]
public class GetCastMemberTest
{
    private readonly CastMemberUseCasesBaseFixture _fixture;

    public GetCastMemberTest(CastMemberUseCasesBaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Application", "GetCastmember - Use Cases")]
    public async Task Get() 
    {
        var examples = _fixture.GetExampleCastMembersList(10);
        var example  = examples[5];

        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.CastMembers.AddRangeAsync(examples);
        await arrangeDbContext.SaveChangesAsync();

        var useCase = new UseCase.GetCastMember(new CastMemberRepository(_fixture.CreateDbContext(true)));
        var input = new UseCase.GetCastMemberInput(example.Id);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(example.Name);
        output.Type.Should().Be(example.Type);
        output.Id.Should().Be(example.Id);
    }
}
