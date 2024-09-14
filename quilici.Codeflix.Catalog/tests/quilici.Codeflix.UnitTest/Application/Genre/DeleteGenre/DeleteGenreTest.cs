using Moq;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.DeleteGenre
{
    [Collection(nameof(DeleteGenreTestFixture))]
    public class DeleteGenreTest
    {
        private readonly DeleteGenreTestFixture _fixture;

        public DeleteGenreTest(DeleteGenreTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(DeleteGenre))]
        [Trait("Aplication", "DeleteGenre - Use cases")]
        public async Task DeleteGenre()
        {
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var exampleGenre = _fixture.GetExampleGenre();

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);

            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object);

            var input = new DeleteGenreInput(exampleGenre.Id);

            await useCase.Handle(input, CancellationToken.None);

            genreRepositoryMock.Verify(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>()), Times.Once);
            genreRepositoryMock.Verify(x => x.Delete(It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
