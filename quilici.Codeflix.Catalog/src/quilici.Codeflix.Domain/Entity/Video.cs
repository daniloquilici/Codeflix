using quilici.Codeflix.Catalog.Domain.Validation;

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

    public Video(string title, string description, bool opened, bool published, int yearLaunched, int druration)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Opened = opened;
        Published = published;
        YearLaunched = yearLaunched;
        Druration = druration;
        CreatedAt = DateTime.Now;
    }

    public void Validate(ValidationHandler handler)
        => new VideoValidator(this, handler).Validate();
}
