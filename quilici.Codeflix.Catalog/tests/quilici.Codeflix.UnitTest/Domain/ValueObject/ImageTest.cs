using FluentAssertions;
using quilici.Codeflix.Catalog.Domain.ValueObject;
using quilici.Codeflix.Catalog.UnitTest.Common;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.ValueObject;
public class ImageTest : BaseFixture
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Image - ValueObjects")]
    public void Instantiate()
    {
        var path = Faker.Image.PicsumUrl();

        var image = new Image(path);

        image.Should().NotBeNull();
        image.Path.Should().NotBeNull();
    }
}
