using Xunit;
using FluentAssertions;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Domain.Entity.Genre
{
    [Collection(nameof(GenreTestFixture))]
    public class GenreTest
    {
        public readonly GenreTestFixture _fixture;

        public GenreTest(GenreTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Genre - Aggregates")]
        public void Instantiate() 
        {
            var genreName = _fixture.GetValidName();
            var dateTimeBefore = DateTime.Now;
            var genre = new DomainEntity.Genre(genreName);
            var dateTimeAfter = DateTime.Now;

            genre.Should().NotBeNull();
            genre.Name.Should().Be(genreName);
            genre.IsActive.Should().BeTrue();
            genre.CreatedAt.Should().NotBeSameDateAs(default);
            (genre.CreatedAt >= dateTimeBefore).Should().BeTrue();
            (genre.CreatedAt <= dateTimeAfter).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Genre - Aggregates")]
        [InlineData(false)]
        [InlineData(true)]
        public void InstantiateWithIsActive(bool isActive)
        {
            var genreName = _fixture.GetValidName();
            var dateTimeBefore = DateTime.Now;
            var genre = new DomainEntity.Genre(genreName, isActive);
            var dateTimeAfter = DateTime.Now;

            genre.Should().NotBeNull();
            genre.Name.Should().Be(genreName);
            genre.IsActive.Should().Be(isActive);
            genre.CreatedAt.Should().NotBeSameDateAs(default);
            (genre.CreatedAt >= dateTimeBefore).Should().BeTrue();
            (genre.CreatedAt <= dateTimeAfter).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(Activate))]
        [Trait("Domain", "Genre - Aggregates")]
        [InlineData(false)]
        [InlineData(true)]
        public void Activate(bool isActive) 
        {
            var genre = _fixture.GetExampleGenre(isActive);
            var oldName = genre.Name;

            genre.Activate();
            
            genre.Should().NotBeNull();
            genre.Name.Should().Be(oldName);
            genre.IsActive.Should().BeTrue();
            genre.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Genre - Aggregates")]
        [InlineData(false)]
        [InlineData(true)]
        public void Deactivate(bool isActive)
        {
            var genre = _fixture.GetExampleGenre(isActive);
            var oldName = genre.Name;

            genre.Deactivate();

            genre.Should().NotBeNull();
            genre.Name.Should().Be(oldName);
            genre.IsActive.Should().BeFalse();
            genre.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Genre - Aggregates")]
        public void Update()
        {
            var genre = _fixture.GetExampleGenre();
            var newName = _fixture.GetValidName();
            var oldIsActive = genre.IsActive;

            genre.Update(newName);

            genre.Should().NotBeNull();
            genre.Name.Should().Be(newName);
            genre.IsActive.Should().Be(oldIsActive);
            genre.CreatedAt.Should().NotBeSameDateAs(default);
        }
    }
}
