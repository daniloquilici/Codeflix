using quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;
using quilici.Codeflix.Catalog.UnitTest.Common.Fixtures;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Video.CreateVideo;

[CollectionDefinition(nameof(CreateVideoTestFixture))]
public class CreateVideoTestFixtureCollection : ICollectionFixture<CreateVideoTestFixture> { }

public class CreateVideoTestFixture : VideoTestFixtureBase
{
    public CreateVideoInput CreateValidCreateVideoInput()
    {
        return new CreateVideoInput(
            "",
            GetValidDescription(),
            GetValidYearLaunched(),
            GetRandomBoolean(),
            GetRandomBoolean(),
            GetValidDuration(),
            GetRandomRating()
            );
    }
}
