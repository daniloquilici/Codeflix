using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using quilici.Codeflix.Catalog.Domain.Repository;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Video.CreateVideo;

[Collection(nameof(CreateVideoTestFixture))]
public class CreateVideoTest
{
    private readonly CreateVideoTestFixture _fixture;

    public CreateVideoTest(CreateVideoTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(Create))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task Create()
    {
        var repositoryMock = new Mock<IVideoRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, repositoryMock.Object);
        var input = _fixture.CreateValidCreateVideoInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.Video>(video =>
            video.Title == input.Title &&
            video.Published == input.Published &&
            video.Description == input.Description &&
            video.Duration == input.Duration &&
            video.Rating == input.Rating &&
            video.Id != Guid.Empty &&
            video.YearLaunched == input.YearLaunched &&
            video.Opened == input.Opened
            ), It.IsAny<CancellationToken>()), Times.Once);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default);
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating);
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
    }

    [Theory(DisplayName = nameof(CreateThrowWithInvalidInput))]
    [Trait("Application", "Create video - Uses Cases")]
    [MemberData(nameof(CreateVideoTestDataGenerator.GetInvalidInputs), 2, MemberType = typeof(CreateVideoTestDataGenerator))]
    public async Task CreateThrowWithInvalidInput(CreateVideoInput input, string expectedValidationError)
    {
        var repositoryMock = new Mock<IVideoRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, repositoryMock.Object);

        var action = async () => await useCase.Handle(input, CancellationToken.None);
        var exceptionAssertion = await action.Should().ThrowAsync<EntityValidationException>();

        exceptionAssertion.WithMessage("There are validation errors").Which.Errors!.ToList()[0].Message.Should().Be(expectedValidationError);

        repositoryMock.Verify(x => x.Insert(It.IsAny<DomainEntity.Video>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
