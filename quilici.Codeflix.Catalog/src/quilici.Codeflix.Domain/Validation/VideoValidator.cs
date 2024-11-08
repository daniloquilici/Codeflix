namespace quilici.Codeflix.Catalog.Domain.Validation;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
public class VideoValidator : Validator
{
    private const int TitleMaxLength = 255;

    private readonly DomainEntity.Video _video;

    public VideoValidator(DomainEntity.Video video, ValidationHandler handler) : base(handler)
    {
        _video = video;
    }

    public override void Validate()
    {
        if (_video.Title.Length > 255)
            _handler.HandleError($"'{_video.Title}' should be less or equal {TitleMaxLength} characters long");
    }
}
