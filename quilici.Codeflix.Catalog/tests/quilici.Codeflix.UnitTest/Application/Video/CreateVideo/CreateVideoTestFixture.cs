using quilici.Codeflix.Catalog.UnitTest.Common.Fixtures;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Video.CreateVideo;

[CollectionDefinition(nameof(CreateVideoTestFixture))]
public class CreateVideoTestFixtureCollection : ICollectionFixture<CreateVideoTestFixture> { }

public class CreateVideoTestFixture : VideoTestFixtureBase
{
}
