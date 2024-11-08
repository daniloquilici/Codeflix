using Xunit;
using FluentAssertions;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.Entity.Video;

[Collection(nameof(VideoTestFixture))]
public class VideoValidatorTest
{
    private readonly VideoTestFixture _fixture;

    public VideoValidatorTest(VideoTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ReturnsValidWhenVideoIsValid))]
    [Trait("Domain", "Video Validator - Validators")]
    public void ReturnsValidWhenVideoIsValid() 
    {
        var validVideo = _fixture.GetValidVideo();
        var notificationValidationHandle = new NotificationValidationHandle();
        var videoValidator = new VideoValidatorTest(videoValidator, notificationValidationHandle);

        videoValidator.Validate();

        notificationValidationHandle.HasErros().Should().BeFalse();
        notificationValidationHandle.GetErrors().Should().HaveCount(0);
    }
}
