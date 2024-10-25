using Xunit;
using DomainEntiry = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.Entity.CastMember;

[Collection(nameof(CastMemberTestFixture))]
public class CastMemberTest
{
    private readonly CastMemberTestFixture _fixture;

    public CastMemberTest(CastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "CastMember - Aggregates")]
    public void Instantiate() 
    {
        var dateTimeBefore = DateTime.Now.AddSeconds(-1);
        var name = "Danilo";
        var type = CastMemberType.Director;

        var castMember = new DomainEntiry.CastMember(name, type);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        castMember.Id.Should().NotBeNull();
        castMember.Name.Should().Be(name);
        castMember.Type.Should().Be(type);
        (castMember.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (castMember.CreatedAt <=> dateTimeAfter).Should().BeTrue();
    }
}
