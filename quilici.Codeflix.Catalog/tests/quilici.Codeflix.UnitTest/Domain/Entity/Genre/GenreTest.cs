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
        public void Instantiate() 
        {
            var genreName = "Horror";
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
    }
}
