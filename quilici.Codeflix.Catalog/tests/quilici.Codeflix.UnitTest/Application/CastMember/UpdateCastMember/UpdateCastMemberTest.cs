using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using quilici.Codeflix.Catalog.Domain.Repository;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.CastMember.UpdateCastMember;

namespace quilici.Codeflix.Catalog.UnitTest.Application.CastMember.UpdateCastMember;

[Collection(nameof(UpdateCastMemberTestFixture))]
public class UpdateCastMemberTest
{
    private readonly UpdateCastMemberTestFixture _fixture;

    public UpdateCastMemberTest(UpdateCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateCastMember))]
    [Trait("Application", "UpdateCastMember - UseCases")]
    public async Task UpdateCastMember()
    {
        var exampleCastMember = _fixture.GetExampleCastMember();

        var unitOfWork = new Mock<IUnitOfWork>();
        var castMemberRepository = new Mock<ICastMemberRepository>();
        castMemberRepository.Setup(x => x.Get(It.Is<Guid>(x => x == exampleCastMember.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleCastMember);

        var input = new UseCase.UpdateCastMemberInput(exampleCastMember.Id, _fixture.GetValidName(), _fixture.GetRandomCastMemberType());
        var useCase = new UseCase.UpdateCastMember(unitOfWork.Object, castMemberRepository.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        castMemberRepository.Verify(x => x.Get(It.Is<Guid>(x => x == exampleCastMember.Id), It.IsAny<CancellationToken>()), Times.Once);
        castMemberRepository.Verify(x => x.Update(It.Is<DomainEntity.CastMember>(x =>
                                                    x.Id == exampleCastMember.Id
                                                    && x.Name == input.Name
                                                    && x.Type == input.Type), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        output.Id.Should().Be(exampleCastMember.Id);
        output.Name.Should().Be(input.Name);
        output.Type.Should().Be(input.Type);
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Application", "UpdateCastMember - UseCases")]
    public async Task ThrowWhenNotFound()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var castMemberRepository = new Mock<ICastMemberRepository>();
        castMemberRepository.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException("notFound"));

        var input = new UseCase.UpdateCastMemberInput(Guid.NewGuid(), _fixture.GetValidName(), _fixture.GetRandomCastMemberType());
        var useCase = new UseCase.UpdateCastMember(unitOfWork.Object, castMemberRepository.Object);

        var action = async () => await useCase.Handle(input, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact(DisplayName = nameof(ThrowWhenInvalidName))]
    [Trait("Application", "UpdateCastMember - UseCases")]
    public async Task ThrowWhenInvalidName()
    {
        var castMemberExample = _fixture.GetExampleCastMember();
        var unitOfWork = new Mock<IUnitOfWork>();
        var castMemberRepository = new Mock<ICastMemberRepository>();
        castMemberRepository.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(castMemberExample);

        var input = new UseCase.UpdateCastMemberInput(Guid.NewGuid(), null!, _fixture.GetRandomCastMemberType());
        var useCase = new UseCase.UpdateCastMember(unitOfWork.Object, castMemberRepository.Object);

        var action = async () => await useCase.Handle(input, CancellationToken.None);
        await action.Should().ThrowAsync<EntityValidationException>().WithMessage("Name should not be empty or null");
    }
}
