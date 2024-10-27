using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Domain.Repository;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.CastMember.GetCastMember;

namespace quilici.Codeflix.Catalog.UnitTest.Application.CastMember.GetCastMember;

[Collection(nameof(GetCastMemberTestFixture))]
public class GetCastMemberTest
{
    private readonly GetCastMemberTestFixture _fixture;

    public GetCastMemberTest(GetCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCastMember))]
    [Trait("Application", "GetCastMember - Use cases")]
    public async Task GetCastMember()
    {
        var casMemberExample = _fixture.GetCastMember();
        var castMemberRepositoryMock = new Mock<ICastMemberRepository>();
        castMemberRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsNotIn<CancellationToken>())).ReturnsAsync(casMemberExample);

        var input = new UseCase.GetCastMemberInput(casMemberExample.Id);
        var useCase = new UseCase.GetCastMember(castMemberRepositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(casMemberExample.Id);
        output.Name.Should().Be(casMemberExample.Name);
        output.Type.Should().Be(casMemberExample.Type);

        castMemberRepositoryMock.Verify(x => x.Get(It.Is<Guid>(x => x == input.Id), It.IsAny<CancellationToken>()), Times.Once());
    }
}
