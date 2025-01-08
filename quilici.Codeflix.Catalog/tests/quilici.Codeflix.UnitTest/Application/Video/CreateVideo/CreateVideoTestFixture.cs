using quilici.Codeflix.Catalog.Application.UseCases.Video.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;
using quilici.Codeflix.Catalog.UnitTest.Common.Fixtures;
using System.Text;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Video.CreateVideo;

[CollectionDefinition(nameof(CreateVideoTestFixture))]
public class CreateVideoTestFixtureCollection : ICollectionFixture<CreateVideoTestFixture> { }

public class CreateVideoTestFixture : VideoTestFixtureBase
{
    public CreateVideoInput CreateValidInput(List<Guid>? categoriesIds = null, List<Guid>? genresIds = null, List<Guid>? castMembersIds = null, FileInput? thumb = null, FileInput? banner = null, FileInput? thumbHalf = null)
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
            genresIds,
            castMembersIds,
            thumb,
            banner,
            thumbHalf);
    }

    public CreateVideoInput CreateValidInputWithAllImages()
    {
        return new CreateVideoInput(
            GetValidTitle(),
            GetValidDescription(),
            GetValidYearLaunched(),
            GetRandomBoolean(),
            GetRandomBoolean(),
            GetValidDuration(),
            GetRandomRating(),
            null,
            null,
            null,
            GetValidImageFileInput(),
            GetValidImageFileInput(),
            GetValidImageFileInput());
    }
}
