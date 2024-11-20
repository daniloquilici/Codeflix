using FluentAssertions;
using quilici.Codeflix.Catalog.Domain.Enum;
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
        var expectedRating = _fixture.GetRandomRating();

        var expectedCreatedDate = DateTime.Now;
        var video = new DomainEntity.Video(expectedTitle, expectedDescription, expectedOpened, expectedPublished, expectedYearLaunched, expectedDuration, expectedRating);

        video.Title.Should().Be(expectedTitle);
        video.Description.Should().Be(expectedDescription);
        video.Opened.Should().Be(expectedOpened);
        video.Published.Should().Be(expectedPublished);
        video.YearLaunched.Should().Be(expectedYearLaunched);
        video.Druration.Should().Be(expectedDuration);
        video.CreatedAt.Should().BeCloseTo(expectedCreatedDate, TimeSpan.FromSeconds(10));
        video.Rating.Should().Be(expectedRating);
        video.Thumb.Should().BeNull();
        video.ThumbHalf.Should().BeNull();
        video.Banner.Should().BeNull();
        video.Media.Should().BeNull();
        video.Trailer.Should().BeNull();
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
            _fixture.GetValidDuration(),
            _fixture.GetRandomRating());

        var notificationValidationHandler = new NotificationValidationHandler();

        video.Validate(notificationValidationHandler);

        notificationValidationHandler.HasErrors().Should().BeTrue();
        notificationValidationHandler.Errors.Should().BeEquivalentTo(new List<ValidationError>()
        {
            new ValidationError("'Title' should be less or equal 255 characters long"),
            new ValidationError("'Description' should be less or equal 4000 characters long")
        });
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Video - Aggregate")]
    public void Update()
    {
        var expectedTitle = _fixture.GetTooLongTitle();
        var expectedDescription = _fixture.GetValidDescription();
        var expectedOpened = _fixture.GetRandomBoolean();
        var expectedPublished = _fixture.GetRandomBoolean();
        var expectedYearLaunched = _fixture.GetValidYearLaunched();
        var expectedDuration = _fixture.GetValidDuration();
        var expectedRating = _fixture.GetRandomRating();

        var video = _fixture.GetValidVideo();
        video.Update(expectedTitle, expectedDescription, expectedOpened, expectedPublished, expectedYearLaunched, expectedDuration, expectedRating);

        video.Title.Should().Be(expectedTitle);
        video.Description.Should().Be(expectedDescription);
        video.Opened.Should().Be(expectedOpened);
        video.Published.Should().Be(expectedPublished);
        video.YearLaunched.Should().Be(expectedYearLaunched);
        video.Druration.Should().Be(expectedDuration);
    }

    [Fact(DisplayName = nameof(ValidadeStillAfterUpdateToValidState))]
    [Trait("Domain", "Video - Aggregate")]
    public void ValidadeStillAfterUpdateToValidState()
    {
        var expectedTitle = _fixture.GetValidTitle();
        var expectedDescription = _fixture.GetValidDescription();
        var expectedOpened = _fixture.GetRandomBoolean();
        var expectedPublished = _fixture.GetRandomBoolean();
        var expectedYearLaunched = _fixture.GetValidYearLaunched();
        var expectedDuration = _fixture.GetValidDuration();
        var expectedRating = _fixture.GetRandomRating();

        var video = _fixture.GetValidVideo();
        video.Update(expectedTitle, expectedDescription, expectedOpened, expectedPublished, expectedYearLaunched, expectedDuration, expectedRating);
        var notificationValidationHandler = new NotificationValidationHandler();
        video.Validate(notificationValidationHandler);

        notificationValidationHandler.HasErrors().Should().BeFalse();
    }

    [Fact(DisplayName = nameof(ValidadeGeneraterErrorsAfterUpdateToInvalidState))]
    [Trait("Domain", "Video - Aggregate")]
    public void ValidadeGeneraterErrorsAfterUpdateToInvalidState()
    {
        var expectedTitle = _fixture.GetTooLongTitle();
        var expectedDescription = _fixture.GetTooLongDescription();
        var expectedOpened = _fixture.GetRandomBoolean();
        var expectedPublished = _fixture.GetRandomBoolean();
        var expectedYearLaunched = _fixture.GetValidYearLaunched();
        var expectedDuration = _fixture.GetValidDuration();
        var expectedRating = _fixture.GetRandomRating();

        var video = _fixture.GetValidVideo();
        video.Update(expectedTitle, expectedDescription, expectedOpened, expectedPublished, expectedYearLaunched, expectedDuration, expectedRating);
        var notificationValidationHandler = new NotificationValidationHandler();
        video.Validate(notificationValidationHandler);

        notificationValidationHandler.HasErrors().Should().BeTrue();
        notificationValidationHandler.Errors.Should().HaveCount(2);
        notificationValidationHandler.Errors.Should().BeEquivalentTo(new List<ValidationError>()
        {
            new ValidationError("'Title' should be less or equal 255 characters long"),
            new ValidationError("'Description' should be less or equal 4000 characters long")
        });
    }

    [Fact(DisplayName = nameof(UpdateThumb))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateThumb()
    {
        var video = _fixture.GetValidVideo();
        var validImagePath = _fixture.GetValidImagePath();

        video.UpdateThumb(validImagePath);
        video.Thumb.Should().NotBeNull();
        video.Thumb!.Path.Should().Be(validImagePath);
    }

    [Fact(DisplayName = nameof(UpdateThumbHalf))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateThumbHalf()
    {
        var video = _fixture.GetValidVideo();
        var validImagePath = _fixture.GetValidImagePath();

        video.UpdateThumbHalf(validImagePath);
        video.ThumbHalf.Should().NotBeNull();
        video.ThumbHalf!.Path.Should().Be(validImagePath);
    }

    [Fact(DisplayName = nameof(UpdateBanner))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateBanner()
    {
        var video = _fixture.GetValidVideo();
        var validImagePath = _fixture.GetValidImagePath();

        video.UpdateBanner(validImagePath);
        video.Banner.Should().NotBeNull();
        video.Banner!.Path.Should().Be(validImagePath);
    }

    [Fact(DisplayName = nameof(UpdateMedia))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateMedia()
    {
        var video = _fixture.GetValidVideo();
        var validPath = _fixture.GetValidMediaPath();

        video.UpdateMedia(validPath);
        video.Media.Should().NotBeNull();
        video.Media!.FilePath.Should().Be(validPath);
    }

    [Fact(DisplayName = nameof(UpdateTrailer))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateTrailer()
    {
        var video = _fixture.GetValidVideo();
        var validPath = _fixture.GetValidMediaPath();

        video.UpdateTrailer(validPath);
        video.Trailer.Should().NotBeNull();
        video.Trailer!.FilePath.Should().Be(validPath);
    }

    [Fact(DisplayName = nameof(UpdateAsSendToEnconde))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateAsSendToEnconde()
    {
        var video = _fixture.GetValidVideo();
        var validPath = _fixture.GetValidMediaPath();

        video.UpdateMedia(validPath);
        video.UpdateAsSentToEncode();
        video.Media.Should().NotBeNull();
        video.Media!.Status.Should().Be(MediaStatus.Processing);
    }

    [Fact(DisplayName = nameof(UpdateAsSendToEncondeThrowsWhenThereIsNoMedia))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateAsSendToEncondeThrowsWhenThereIsNoMedia()
    {
        var video = _fixture.GetValidVideo();

        var action = () => video.UpdateAsSentToEncode();
        action.Should().Throw<EntityValidationException>().WithMessage("There is no Media");
    }

    [Fact(DisplayName = nameof(UpdateAsEncoded))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateAsEncoded()
    {
        var video = _fixture.GetValidVideo();
        var validPath = _fixture.GetValidMediaPath();
        var validEncodedPath = _fixture.GetValidMediaPath();

        video.UpdateMedia(validPath);
        video.UpdateAsEncoded(validEncodedPath);
        video.Media.Should().NotBeNull();
        video.Media!.Status.Should().Be(MediaStatus.Completed);
        video.Media!.EncodedPath.Should().Be(validEncodedPath);
    }

    [Fact(DisplayName = nameof(UpdateAsEncodedThrowsWhenThereIsNoMedia))]
    [Trait("Domain", "Video - Aggregate")]
    public void UpdateAsEncodedThrowsWhenThereIsNoMedia()
    {
        var video = _fixture.GetValidVideo();
       
        var validEncodedPath = _fixture.GetValidMediaPath();

        var action = () => video.UpdateAsEncoded(validEncodedPath);
        action.Should().Throw<EntityValidationException>().WithMessage("There is no Media");
    }
}
