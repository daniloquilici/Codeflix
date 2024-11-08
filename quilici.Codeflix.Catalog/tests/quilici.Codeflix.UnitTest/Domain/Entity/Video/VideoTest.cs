using FluentAssertions;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.Entity.Video;

[Collection(nameof(VideoTestFixture))]
public class VideoTest
{
    private readonly VideoTestFixture _fixture;

    public VideoTest(VideoTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Video - Aggregate")]
    public void Instantiate()
    {
        var expectedTitle = _fixture.GetTooLongTitle();
        var expectedDescription = _fixture.GetValidDescription();
        var expectedOpened = _fixture.GetRandomBoolean();
        var expectedPublished = _fixture.GetRandomBoolean();
        var expectedYearLaunched = _fixture.GetValidYearLaunched();
        var expectedDuration = _fixture.GetValidDuration();

        var video = new DomainEntity.Video(expectedTitle, expectedDescription, expectedOpened, expectedPublished, expectedYearLaunched, expectedDuration);

        video.Title.Should().Be(expectedTitle);
        video.Description.Should().Be(expectedDescription);
        video.Opened.Should().Be(expectedOpened);
        video.Published.Should().Be(expectedPublished);
        video.YearLaunched.Should().Be(expectedYearLaunched);
        video.Druration.Should().Be(expectedDuration);
    }
}
