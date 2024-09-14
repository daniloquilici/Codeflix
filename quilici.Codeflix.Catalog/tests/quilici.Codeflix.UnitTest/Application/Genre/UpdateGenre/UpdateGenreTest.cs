using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.UpdateGenre
{
    [Collection(nameof(UpdateGenreTestFixture))]
    public class UpdateGenreTest
    {
        private readonly UpdateGenreTestFixture _fixture;

        public UpdateGenreTest(UpdateGenreTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(UpdateGenre))]
        [Trait("Aplication", "UpdateGenre - Use cases")]
        public async Task UpdateGenre()
        {
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var exampleGenre = _fixture.GetExampleGente();
            var newNameExample = _fixture.GetValidGenreName();
            var newIsActiove = !exampleGenre.IsActive;

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);

            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object, _fixture.GetCategoryRepositoryMock().Object);

            var input = new UpdateGenreInput(exampleGenre.Id, newNameExample, newIsActiove);

            GenreModelOuput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(exampleGenre.Id);
            output.Name.Should().Be(newNameExample);
            output.IsActive.Should().Be(newIsActiove);
            output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
            output.Categories.Should().HaveCount(0);

            genreRepositoryMock.Verify(x => x.Update(It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = nameof(ThrowWhenNotFound))]
        [Trait("Aplication", "UpdateGenre - Use cases")]
        public async Task ThrowWhenNotFound()
        {
            var exampleId = Guid.NewGuid();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();

            genreRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Throws(new NotFoundException($"Genre '{exampleId}' not found."));

            var useCase = new UseCase.UpdateGenre(_fixture.GetUnitOfWorkMock().Object, genreRepositoryMock.Object, _fixture.GetCategoryRepositoryMock().Object);

            var input = new UpdateGenreInput(exampleId, _fixture.GetValidGenreName(), true);

            var action = async () => await useCase.Handle(input, CancellationToken.None);
            await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{exampleId}' not found.");
        }

        [Theory(DisplayName = nameof(ThrowWhenNameIsInvalid))]
        [Trait("Aplication", "UpdateGenre - Use cases")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task ThrowWhenNameIsInvalid(string? name)
        {
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var exampleGenre = _fixture.GetExampleGente();
            var newNameExample = _fixture.GetValidGenreName();
            var newIsActiove = !exampleGenre.IsActive;

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);

            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object, _fixture.GetCategoryRepositoryMock().Object);

            var input = new UpdateGenreInput(exampleGenre.Id, name!, newIsActiove);

            var action = async () => await useCase.Handle(input, CancellationToken.None);
            await action.Should().ThrowAsync<EntityValidationException>().WithMessage($"Name should not be empty or null");
        }

        [Theory(DisplayName = nameof(UpdateGenreOnlyName))]
        [Trait("Aplication", "UpdateGenre - Use cases")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateGenreOnlyName(bool isActive)
        {
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var exampleGenre = _fixture.GetExampleGente(isActive);
            var newNameExample = _fixture.GetValidGenreName();
            var newIsActiove = !exampleGenre.IsActive;

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);

            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object, _fixture.GetCategoryRepositoryMock().Object);

            var input = new UpdateGenreInput(exampleGenre.Id, newNameExample);

            GenreModelOuput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(exampleGenre.Id);
            output.Name.Should().Be(newNameExample);
            output.IsActive.Should().Be(isActive);
            output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
            output.Categories.Should().HaveCount(0);

            genreRepositoryMock.Verify(x => x.Update(It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
