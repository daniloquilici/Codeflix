using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Domain.Repository;
using System.Linq.Expressions;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.CastMember.DeleteCastMember;

namespace quilici.Codeflix.Catalog.UnitTest.Application.CastMember.DeleteCastMember;

[Collection(nameof(DeleteCastMemberTestFixture))]
public class DeleteCastMemberTest
{
    private readonly DeleteCastMemberTestFixture _fixture;

    public DeleteCastMemberTest(DeleteCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCastMember))]
    [Trait("Application", "DeleteCastMember - Use cases")]
    public async Task DeleteCastMember()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var castMemberRepositoryMock = new Mock<ICastMemberRepository>();
        var castMemberExample = _fixture.GetExampleCastMember();

        castMemberRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(castMemberExample);

        var input = new UseCase.DeleteCastMemberInput(castMemberExample.Id);
        var useCase = new UseCase.DeleteCastMember(unitOfWorkMock.Object, castMemberRepositoryMock.Object);

        var action = async () => await useCase.Handle(input, CancellationToken.None);
        await action.Should().NotThrowAsync();

        castMemberRepositoryMock.Verify(x => x.Get(It.Is<Guid>(x => x == input.Id), It.IsAny<CancellationToken>()), Times.Once);
        castMemberRepositoryMock.Verify(x => x.Delete(It.Is<DomainEntity.CastMember>(x => x.Id == input.Id), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowsWhenNotFound))]
    [Trait("Application", "DeleteCastMember - Use cases")]
    public async Task ThrowsWhenNotFound()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var castMemberRepositoryMock = new Mock<ICastMemberRepository>();

        castMemberRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException("notFound"));

        var input = new UseCase.DeleteCastMemberInput(Guid.NewGuid());
        var useCase = new UseCase.DeleteCastMember(unitOfWorkMock.Object, castMemberRepositoryMock.Object);

        var action = async () => await useCase.Handle(input, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
