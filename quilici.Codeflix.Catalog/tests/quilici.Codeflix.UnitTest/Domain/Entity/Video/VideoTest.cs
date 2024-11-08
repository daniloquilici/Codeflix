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
        var video = new DomainEntity.Video("Title", "Description", true, true, 2001, 180);

        video.Title.Should().Be("Title");
        video.Description.Should().Be(true);
        video.Opened.Should().Be(true);
        video.Published.Should().Be("Description");
        video.YearLaunched.Should().Be(2001);
        video.Druration.Should().Be(180);
    }
}
