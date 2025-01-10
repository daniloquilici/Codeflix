using quilici.Codeflix.Catalog.UnitTest.Common.Fixtures;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Video.UploadMedias;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Video.UploadMedias;

[CollectionDefinition(nameof(UploadMediasTestFixture))]
public class UploadMediasTestFixtureCollection : ICollectionFixture<UploadMediasTestFixture> { }

public class UploadMediasTestFixture : VideoTestFixtureBase
{
    public UseCase.UploadMediasInput GetValidInput()
    {
        return new(Guid.NewGuid(), GetValidMediaFileInput(), GetValidMediaFileInput());
    }
}
