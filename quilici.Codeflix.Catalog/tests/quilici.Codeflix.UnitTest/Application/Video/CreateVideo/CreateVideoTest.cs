using Moq;
using quilici.Codeflix.Catalog.Application.Interfaces;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity.Video;
using quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Video.CreateVideo;

[Collection(nameof(CreateVideoTestFixture))]
public class CreateVideoTest
{
    private readonly CreateVideoTestFixture _fixture;

    public CreateVideoTest(CreateVideoTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(Create))]
    [Trait("Application", "")]
    public async Task Create()
    {
        var repositoryMock = new Mock<IVideoRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, repositoryMock.Object);
        var input = new CreateVideoInput(
            _fixture.GetValidTitle(),
            _fixture.GetValidDescription(),
            _fixture.GetValidYearLaunched(),
            _fixture.GetRandomBoolean(),
            _fixture.GetRandomBoolean(),
            _fixture.GetValidDuration(),
            _fixture.GetRandomRating()
            );

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.Video>(video =>
            video.Title == input.Title &&
            video.Published == input.Published &&
            video.Description == input.Description &&
            video.Duration == input.Duration &&
            video.Rating == input.Rating &&
            video.Id == input.Id &&
            video.YearLauched == input.YearLauched &&
            video.Opened == input.Opened
            ), It.IsAny<CancellationToken>()), Times.Once);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeEmpty();
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration)
        output.Rating.Should().Be(input.Rating);
        output.YearLauched.Should().Be(input.YearLauched);
        output.Opened.Should().Be(input.Openedoutput);
    }
}
