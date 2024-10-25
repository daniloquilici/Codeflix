using FluentAssertions;
using quilici.Codeflix.Catalog.Domain.Enum;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

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
        var name = _fixture.GetValidName();
        var type = _fixture.GetRandomCastMemberType();

        var castMember = new DomainEntity.CastMember(name, type);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        castMember.Id.Should().NotBeEmpty();
        castMember.Name.Should().Be(name);
        castMember.Type.Should().Be(type);
        (castMember.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (castMember.CreatedAt <= dateTimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(ThrowErrorWhenNameIsInvalid))]
    [Trait("Domain", "CastMember - Aggregates")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ThrowErrorWhenNameIsInvalid(string? name)
    {
        var type = _fixture.GetRandomCastMemberType();
       
        var action = () => new DomainEntity.CastMember(name!, type);
        action.Should().Throw<EntityValidationException>().WithMessage($"Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "CastMember - Aggregates")]
    public void Update()
    {
        var newName = _fixture.GetValidName();
        var newType = _fixture.GetRandomCastMemberType();
        var castMember = _fixture.GetExempleCastMember();

        castMember.Update(newName, newType);

        castMember.Name.Should().Be(newName);
        castMember.Type.Should().Be(newType);
    }

    [Theory(DisplayName = nameof(UpdateThrowsErrorWhenNameIsInvalid))]
    [Trait("Domain", "CastMember - Aggregates")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateThrowsErrorWhenNameIsInvalid(string? newName)
    {
        var newType = _fixture.GetRandomCastMemberType();
        var castMember = _fixture.GetExempleCastMember();

        var action = () => castMember.Update(newName!, newType);
        action.Should().Throw<EntityValidationException>().WithMessage($"Name should not be empty or null");
    }
}
