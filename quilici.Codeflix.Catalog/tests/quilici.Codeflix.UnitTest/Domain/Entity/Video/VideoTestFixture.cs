using quilici.Codeflix.Catalog.UnitTest.Common;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.Entity.Video;

[CollectionDefinition(nameof(VideoTestFixture))]
public class VideoTestFixtureCollection : ICollectionFixture<VideoTestFixture> { }

public class VideoTestFixture : BaseFixture
{
    public DomainEntity.Video GetValidVideo()
        => new DomainEntity.Video("Title", "Description", true, true, 2001, 180);

    public string GetValidTitle()
        => Faker.Lorem.Letter(100);

    public string GetTooLongTitle()
    => Faker.Lorem.Letter(400);

    public string GetValidDescription()
        => Faker.Commerce.ProductName();

    public int GetValidYearLaunched()
        => Faker.Date.BetweenDateOnly(new DateOnly(1960, 1, 1), new DateOnly(2022, 1, 1)).Year;

    public int GetValidDuration()
        => (new Random()).Next(100, 300);
}
