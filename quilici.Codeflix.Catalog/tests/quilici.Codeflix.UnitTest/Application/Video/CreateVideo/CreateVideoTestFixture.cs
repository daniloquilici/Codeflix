using quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;
using quilici.Codeflix.Catalog.UnitTest.Common.Fixtures;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Video.CreateVideo;

[CollectionDefinition(nameof(CreateVideoTestFixture))]
public class CreateVideoTestFixtureCollection : ICollectionFixture<CreateVideoTestFixture> { }

public class CreateVideoTestFixture : VideoTestFixtureBase
{
    public CreateVideoInput CreateValidCreateVideoInput(List<Guid>? categoriesIds = null, List<Guid>? genresIds = null)
    {
        return new CreateVideoInput(
            GetValidTitle(),
            GetValidDescription(),
            GetValidYearLaunched(),
            GetRandomBoolean(),
            GetRandomBoolean(),
            GetValidDuration(),
            GetRandomRating(),
            categoriesIds,
            genresIds);
    }
}
