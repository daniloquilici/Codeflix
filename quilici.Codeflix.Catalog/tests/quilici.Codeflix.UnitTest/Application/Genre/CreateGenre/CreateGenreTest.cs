using FluentAssertions;
using Moq;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using DomianEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.CreateGenre
{
    [Collection(nameof(CreateGenreTestFixture))]
    public class CreateGenreTest
    {
        private readonly CreateGenreTestFixture _fixture;

        public CreateGenreTest(CreateGenreTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(CreateGenre))]
        [Trait("Aplication", "CreateGenre - Use cases")]
        public async Task CreateGenre()
        {
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            var usesCases = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object);

            var input = _fixture.GetExampleInput();

            var dateTimeBefore = DateTime.Now;
            var output = await usesCases.Handle(input, CancellationToken.None);
            var dateTimeAfter = DateTime.Now;

            genreRepositoryMock.Verify(x => x.Insert(It.IsAny<DomianEntity.Genre>(),
                                                     It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.Id.Should().NotBeEmpty();
            output.Name.Should().Be(input.Name);
            output.IsActive.Should().Be(input.IsActive);
            output.Categories.Should().HaveCount(0);
            output.CreatedAt.Should().NotBeSameDateAs(default);
            (output.CreatedAt >= dateTimeBefore).Should().BeTrue();
            (output.CreatedAt <= dateTimeAfter).Should().BeTrue();
        }
    }
}
