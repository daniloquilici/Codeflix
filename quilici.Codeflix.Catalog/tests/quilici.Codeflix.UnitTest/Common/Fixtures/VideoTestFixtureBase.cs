using quilici.Codeflix.Catalog.Domain.Entity;
using quilici.Codeflix.Catalog.Domain.Enum;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Common.Fixtures;

public abstract class VideoTestFixtureBase : BaseFixture
{
    public DomainEntity.Video GetValidVideo()
        => new DomainEntity.Video(GetValidTitle(), GetValidDescription(), GetRandomBoolean(), GetRandomBoolean(), GetValidYearLaunched(), GetValidDuration(), GetRandomRating());

    public Rating GetRandomRating()
    {
        var values = Enum.GetValues<Rating>();
        var random = new Random();
        return values[random.Next(values.Length)];
    }

    public string GetValidTitle()
        => Faker.Lorem.Letter(100);

    public string GetTooLongTitle()
        => Faker.Lorem.Letter(400);

    public string GetTooLongDescription()
        => Faker.Lorem.Letter(4001);

    public string GetValidDescription()
        => Faker.Commerce.ProductName();

    public int GetValidYearLaunched()
        => Faker.Date.BetweenDateOnly(new DateOnly(1960, 1, 1), new DateOnly(2022, 1, 1)).Year;

    public int GetValidDuration()
        => (new Random()).Next(100, 300);

    public string GetValidImagePath()
        => Faker.Image.PlaceImgUrl();

    public string GetValidMediaPath()
    {
        var examplesMedias = new string[]
        {
            "https://www.googlestorage.com/file-example.mp4",
            "https://www.storage.com/another-example-of-video.mp4",
            "https://www.S3.com.br/example.mp4",
            "https://www.glg.io/file.mp4",
        };
        var random = new Random();
        return examplesMedias[random.Next(examplesMedias.Length)];
    }

    public Media GetValidMedia()
        => new(GetValidMediaPath());
}
