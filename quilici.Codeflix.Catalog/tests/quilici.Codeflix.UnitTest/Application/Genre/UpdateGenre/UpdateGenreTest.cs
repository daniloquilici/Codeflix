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
            var exampleGenre = _fixture.GetExampleGenre();
            var newNameExample = _fixture.GetValidGenreName();
            var newIsActiove = !exampleGenre.IsActive;

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);

            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object, _fixture.GetCategoryRepositoryMock().Object);

            var input = new UpdateGenreInput(exampleGenre.Id, newNameExample, newIsActiove);

            GenreModelOutput output = await useCase.Handle(input, CancellationToken.None);

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
            var exampleGenre = _fixture.GetExampleGenre();
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
            var exampleGenre = _fixture.GetExampleGenre(isActive);
            var newNameExample = _fixture.GetValidGenreName();
            var newIsActiove = !exampleGenre.IsActive;

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);

            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object, _fixture.GetCategoryRepositoryMock().Object);

            var input = new UpdateGenreInput(exampleGenre.Id, newNameExample);

            GenreModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(exampleGenre.Id);
            output.Name.Should().Be(newNameExample);
            output.IsActive.Should().Be(isActive);
            output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
            output.Categories.Should().HaveCount(0);

            genreRepositoryMock.Verify(x => x.Update(It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = nameof(UpdateGenreAddingCategoriesIds))]
        [Trait("Aplication", "UpdateGenre - Use cases")]
        public async Task UpdateGenreAddingCategoriesIds()
        {
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
            var exampleGenre = _fixture.GetExampleGenre();
            var newNameExample = _fixture.GetValidGenreName();
            var newIsActiove = !exampleGenre.IsActive;
            var exampleCategoriesIdsList = _fixture.GetRandomIdsList();

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);
            categoryRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategoriesIdsList);

            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object, categoryRepositoryMock.Object);

            var input = new UpdateGenreInput(exampleGenre.Id, newNameExample, newIsActiove, exampleCategoriesIdsList);

            GenreModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(exampleGenre.Id);
            output.Name.Should().Be(newNameExample);
            output.IsActive.Should().Be(newIsActiove);
            output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
            output.Categories.Should().HaveCount(exampleCategoriesIdsList.Count);
            exampleCategoriesIdsList.ForEach(expectedId => output.Categories.Should().Contain(relation => relation.Id == expectedId));

            genreRepositoryMock.Verify(x => x.Update(It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = nameof(UpdateGenreReplacingCategoriesIds))]
        [Trait("Aplication", "UpdateGenre - Use cases")]
        public async Task UpdateGenreReplacingCategoriesIds()
        {
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
            var exampleGenre = _fixture.GetExampleGenre(categoriesIds: _fixture.GetRandomIdsList());
            var newNameExample = _fixture.GetValidGenreName();
            var newIsActiove = !exampleGenre.IsActive;
            var exampleCategoriesIdsList = _fixture.GetRandomIdsList();

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);
            categoryRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategoriesIdsList);

            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object, categoryRepositoryMock.Object);

            var input = new UpdateGenreInput(exampleGenre.Id, newNameExample, newIsActiove, exampleCategoriesIdsList);

            GenreModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(exampleGenre.Id);
            output.Name.Should().Be(newNameExample);
            output.IsActive.Should().Be(newIsActiove);
            output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
            output.Categories.Should().HaveCount(exampleCategoriesIdsList.Count);
            exampleCategoriesIdsList.ForEach(expectedId => output.Categories.Should().Contain(relation => relation.Id == expectedId));

            genreRepositoryMock.Verify(x => x.Update(It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = nameof(ThrowWhenCategorieNotFound))]
        [Trait("Aplication", "UpdateGenre - Use cases")]
        public async Task ThrowWhenCategorieNotFound()
        {
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

            var exampleGenre = _fixture.GetExampleGenre(categoriesIds: _fixture.GetRandomIdsList());
            var newNameExample = _fixture.GetValidGenreName();
            var newIsActiove = !exampleGenre.IsActive;
            var exampleNewCategoriesIdsList = _fixture.GetRandomIdsList(10);
            var listReturnedByCategoryRepository = exampleNewCategoriesIdsList.GetRange(0, exampleNewCategoriesIdsList.Count - 2);
            var idsNotReturnedByCategoryRepository = exampleNewCategoriesIdsList.GetRange(exampleNewCategoriesIdsList.Count - 2, 2);

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);
            categoryRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(listReturnedByCategoryRepository);

            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object, categoryRepositoryMock.Object);

            var input = new UpdateGenreInput(exampleGenre.Id, newNameExample, newIsActiove, exampleNewCategoriesIdsList);

            var action = async () => await useCase.Handle(input, CancellationToken.None);

            var notFoundIdsAsString = string.Join(", ", idsNotReturnedByCategoryRepository);
            await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage($"Related category Id(s) not found: {notFoundIdsAsString}");
        }

        [Fact(DisplayName = nameof(UpdateGenreWithoutCategoriesIds))]
        [Trait("Aplication", "UpdateGenre - Use cases")]
        public async Task UpdateGenreWithoutCategoriesIds()
        {
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

            var exampleCategoriesIdsList = _fixture.GetRandomIdsList();
            var exampleGenre = _fixture.GetExampleGenre(categoriesIds: exampleCategoriesIdsList);
            var newNameExample = _fixture.GetValidGenreName();
            var newIsActiove = !exampleGenre.IsActive;            

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);
            
            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object, categoryRepositoryMock.Object);

            var input = new UpdateGenreInput(exampleGenre.Id, newNameExample, newIsActiove);

            GenreModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(exampleGenre.Id);
            output.Name.Should().Be(newNameExample);
            output.IsActive.Should().Be(newIsActiove);
            output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
            output.Categories.Should().HaveCount(exampleCategoriesIdsList.Count);
            exampleCategoriesIdsList.ForEach(expectedId => output.Categories.Should().Contain(relation => relation.Id == expectedId));

            genreRepositoryMock.Verify(x => x.Update(It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = nameof(UpdateGenreWithEmptyCategoriesIdsLislt))]
        [Trait("Aplication", "UpdateGenre - Use cases")]
        public async Task UpdateGenreWithEmptyCategoriesIdsLislt()
        {
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();

            var exampleCategoriesIdsList = _fixture.GetRandomIdsList();
            var exampleGenre = _fixture.GetExampleGenre(categoriesIds: exampleCategoriesIdsList);
            var newNameExample = _fixture.GetValidGenreName();
            var newIsActiove = !exampleGenre.IsActive;

            genreRepositoryMock.Setup(x => x.Get(It.Is<Guid>(x => x == exampleGenre.Id), It.IsAny<CancellationToken>())).ReturnsAsync(exampleGenre);

            var useCase = new UseCase.UpdateGenre(unitOfWorkMock.Object, genreRepositoryMock.Object, categoryRepositoryMock.Object);

            var input = new UpdateGenreInput(exampleGenre.Id, newNameExample, newIsActiove, new List<Guid>());

            GenreModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(exampleGenre.Id);
            output.Name.Should().Be(newNameExample);
            output.IsActive.Should().Be(newIsActiove);
            output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
            output.Categories.Should().HaveCount(0);

            genreRepositoryMock.Verify(x => x.Update(It.Is<DomainEntity.Genre>(x => x.Id == exampleGenre.Id), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
