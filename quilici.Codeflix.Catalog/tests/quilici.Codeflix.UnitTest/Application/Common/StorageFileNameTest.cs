using FluentAssertions;
using quilici.Codeflix.Catalog.Application.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Common;
public class StorageFileNameTest
{
    [Fact(DisplayName = nameof(CreateStorageFileName))]
    [Trait("Application", "StorageName - Common")]
    public void CreateStorageFileName()
    {
        var exampleId = Guid.NewGuid();
        var exampleExtention = "mp4";
        var propertyName = "Video";

        var name = StorageFileName.Create(exampleId, propertyName, exampleExtention);

        name.Should().Be($"{exampleId}-{propertyName.ToLower()}.{exampleExtention}");
    }
}
