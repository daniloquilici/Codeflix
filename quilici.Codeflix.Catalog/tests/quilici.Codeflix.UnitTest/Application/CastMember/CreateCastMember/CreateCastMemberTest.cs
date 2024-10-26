using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using quilici.Codeflix.Catalog.Domain.Repository;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;

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
    public async Task Create()
    {
        var input = new CreateCastMemberInput(_fixture.GetValidName(), _fixture.GetRandomCastMemberType());

        var repositoryMock = new Mock<ICastMemberRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var useCase = new UseCase.CreateCastMember(unitOfWork.Object, repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Type.Should().Be(input.Type);
        ((DateTime)output.CreatedAt).Should().NotBeSameDateAs(default);

        unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.CastMember>(x => x.Name == input.Name && x.Type == input.Type), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(ThrowsWhenInvalidName))]
    [Trait("Aplication", "CreateCastMember - Use cases")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public async Task ThrowsWhenInvalidName(string? name)
    {
        var input = new CreateCastMemberInput(name!, _fixture.GetRandomCastMemberType());

        var repositoryMock = new Mock<ICastMemberRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var useCase = new UseCase.CreateCastMember(unitOfWork.Object, repositoryMock.Object);

        var action = async () => await useCase.Handle(input, CancellationToken.None);
        await action.Should().ThrowAsync<EntityValidationException>().WithMessage($"Name should not be empty or null");
    }
}
