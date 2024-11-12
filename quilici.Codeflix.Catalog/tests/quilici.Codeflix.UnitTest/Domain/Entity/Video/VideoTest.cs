using FluentAssertions;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using quilici.Codeflix.Catalog.Domain.Validation;
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

        var expectedCreatedDate = DateTime.Now;
        var video = new DomainEntity.Video(expectedTitle, expectedDescription, expectedOpened, expectedPublished, expectedYearLaunched, expectedDuration);

        video.Title.Should().Be(expectedTitle);
        video.Description.Should().Be(expectedDescription);
        video.Opened.Should().Be(expectedOpened);
        video.Published.Should().Be(expectedPublished);
        video.YearLaunched.Should().Be(expectedYearLaunched);
        video.Druration.Should().Be(expectedDuration);
        video.CreatedAt.Should().BeCloseTo(expectedCreatedDate, TimeSpan.FromSeconds(10));
    }

    [Fact(DisplayName = nameof(ValidateWhenValidState))]
    [Trait("Domain", "Video - Aggregate")]
    public void ValidateWhenValidState()
    {
        var video = _fixture.GetValidVideo();
        var notificationValidationHandler = new NotificationValidationHandler();

        video.Validate(notificationValidationHandler);
        
        notificationValidationHandler.HasErrors().Should().BeFalse();
    }

    [Fact(DisplayName = nameof(ValidateWithErrorWhenInvalidState))]
    [Trait("Domain", "Video - Aggregate")]
    public void ValidateWithErrorWhenInvalidState()
    {
        var video = new DomainEntity.Video(
            _fixture.GetTooLongTitle(),
            _fixture.GetTooLongDescription(),
            _fixture.GetRandomBoolean(),
            _fixture.GetRandomBoolean(),
            _fixture.GetValidYearLaunched(),
            _fixture.GetValidDuration());

        var notificationValidationHandler = new NotificationValidationHandler();

        video.Validate(notificationValidationHandler);

        notificationValidationHandler.HasErrors().Should().BeTrue();
        notificationValidationHandler.Errors.Should().BeEquivalentTo(new List<ValidationError>() 
        {
            new ValidationError("'Title' should be less or equal 255 characters long"),
            new ValidationError("'Description' should be less or equal 4000 characters long")
        });
    }
}
