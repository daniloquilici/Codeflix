namespace quilici.Codeflix.Catalog.Domain.Validation;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
public class VideoValidator : Validator
{
    private const int TitleMaxLength = 255;
    private const int DescriptionMaxLength = 400;

    private readonly DomainEntity.Video _video;

    public VideoValidator(DomainEntity.Video video, ValidationHandler handler) : base(handler)
    {
        _video = video;
    }

    public override void Validate()
    {
        ValidateTitle();

        ValidateDescription();
    }

    private void ValidateTitle()
    {
        if (string.IsNullOrWhiteSpace(_video.Title))
            _handler.HandleError($"'{nameof(_video.Title)}' is requered");

        if (_video.Title.Length > 255)
            _handler.HandleError($"'{nameof(_video.Title)}' should be less or equal {TitleMaxLength} characters long");
    }

    private void ValidateDescription() 
    {
        if (string.IsNullOrWhiteSpace(_video.Description))
            _handler.HandleError($"'{nameof(_video.Description)}' is requered");

        if (_video.Description.Length > DescriptionMaxLength)
            _handler.HandleError($"'{nameof(_video.Description)}' should be less or equal {DescriptionMaxLength} characters long");
    }
}
