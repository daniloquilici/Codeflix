using quilici.Codeflix.Catalog.Domain.Enum;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using quilici.Codeflix.Catalog.Domain.Validation;
using quilici.Codeflix.Catalog.Domain.ValueObject;

namespace quilici.Codeflix.Catalog.Domain.Entity;
public class Video
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public bool Opened { get; private set; }

    public bool Published { get; private set; }

    public int YearLaunched { get; private set; }

    public int Druration { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Rating Rating { get; private set; }

    public Image? Thumb { get; private set; }
    
    public Image? ThumbHalf { get; private set; }
    
    public Image? Banner { get; private set; }

    public Media? Media { get; private set; }
    
    public Media? Trailer { get; private set; }

    public Video(string title, string description, bool opened, bool published, int yearLaunched, int druration, Rating rating)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Opened = opened;
        Published = published;
        YearLaunched = yearLaunched;
        Druration = druration;
        CreatedAt = DateTime.Now;
        Rating = rating;
    }

    public void Update(string title, string description, bool opened, bool published, int yearLaunched, int druration, Rating rating)
    {
        Title = title;
        Description = description;
        Opened = opened;
        Published = published;
        YearLaunched = yearLaunched;
        Druration = druration;
        Rating = rating;
    }

    public void Validate(ValidationHandler handler)
        => new VideoValidator(this, handler).Validate();

    public void UpdateThumb(string validImagePath) 
        => Thumb = new Image(validImagePath);

    public void UpdateThumbHalf(string validImagePath)
        => ThumbHalf = new Image(validImagePath);

    public void UpdateBanner(string validImagePath) 
        => Banner = new Image(validImagePath);

    public void UpdateMedia(string validPath)
        => Media = new Media(validPath);

    public void UpdateTrailer(string validPath)
        => Trailer = new Media(validPath);

    public void UpdateAsSentToEncode()
    {
        if (Media is null)
            throw new EntityValidationException("There is no Media");

        Media!.UpdateAsSentToEncode(); 
    }

    public void UpdateAsEncoded(string validEncodedPath)
    {
        if (Media is null)
            throw new EntityValidationException("There is no Media");
        
        Media!.UpdateAsEncoded(validEncodedPath);
    }
}
