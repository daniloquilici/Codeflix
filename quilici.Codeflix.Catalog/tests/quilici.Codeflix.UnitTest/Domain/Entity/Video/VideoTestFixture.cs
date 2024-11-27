using quilici.Codeflix.Catalog.UnitTest.Common.Fixtures;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.Entity.Video;

[CollectionDefinition(nameof(VideoTestFixture))]
public class VideoTestFixtureCollection : ICollectionFixture<VideoTestFixture> { }

public class VideoTestFixture : VideoTestFixtureBase
{
}
