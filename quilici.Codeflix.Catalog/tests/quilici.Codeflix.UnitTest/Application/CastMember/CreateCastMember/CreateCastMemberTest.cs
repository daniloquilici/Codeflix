using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Domain.Enum;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Application.CastMember.CreateCastMember;

[Collection(nameof(CreateCastMemberTestFixture))]
public class CreateCastMemberTest
{
    private readonly CreateCastMemberTestFixture _fixture;

    public CreateCastMemberTest(CreateCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Create))]
    [Trait("Aplication", "CreateCastMember - Use cases")]
    public void Create()
    {
        var input = new CreateCastMemberInput("Danilo", CastMemberType.Director);

        var repositoryMock = new Mock<ICastMemberRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var useCase = new CreateCastMember(repositoryMock, unitOfWork);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Type.Should().Be(input.Type);
        ((DateTime)output.CreatedAt).Should().NotBeSameDateAs(default);

        unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.CastMember>(x => x.Name == input.Name && x.Type == input.Type), It.IsAny<CancellationToken>()), Times.Once);
    }
}
