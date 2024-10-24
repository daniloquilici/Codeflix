﻿using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using Xunit;
using DomianEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;

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
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            var usesCases = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object, categoryRepositoryMock.Object);

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

        [Fact(DisplayName = nameof(CreateWithRelatedCategories))]
        [Trait("Aplication", "CreateGenre - Use cases")]
        public async Task CreateWithRelatedCategories()
        {
            var input = _fixture.GetExampleInputWithCategories();
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            categoryRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((IReadOnlyList<Guid>)input.CategoriesIds!);

            var usesCases = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object, categoryRepositoryMock.Object);

            

            var output = await usesCases.Handle(input, CancellationToken.None);

            genreRepositoryMock.Verify(x => x.Insert(It.IsAny<DomianEntity.Genre>(),
                                                     It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            output.Should().NotBeNull();
            output.Id.Should().NotBeEmpty();
            output.Name.Should().Be(input.Name);
            output.IsActive.Should().Be(input.IsActive);
            output.Categories.Should().HaveCount(input.CategoriesIds?.Count ?? 0);
            input.CategoriesIds?.ForEach(id => output.Categories.Should().Contain(relation => relation.Id == id));
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(CreateThrowWhenRelatedCategoryNotFound))]
        [Trait("Aplication", "CreateGenre - Use cases")]
        public async Task CreateThrowWhenRelatedCategoryNotFound()
        {
            var input = _fixture.GetExampleInputWithCategories();
            var exampleGuid = input.CategoriesIds![^1];
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            categoryRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((IReadOnlyList<Guid>)input.CategoriesIds.FindAll(x => x != exampleGuid));


            var usesCases = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object, categoryRepositoryMock.Object);

            var action = async () => await usesCases.Handle(input, CancellationToken.None);
            await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage($"Related category Id(s) not found: {exampleGuid}");
            categoryRepositoryMock.Verify(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = nameof(CreateThrowWhenRelatedCategoryNotFound))]
        [Trait("Aplication", "CreateGenre - Use cases")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task ThrowWhenNameIsInvalid(string? name)
        {
            var input = _fixture.GetExampleInput(name);
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            var usesCases = new UseCase.CreateGenre(genreRepositoryMock.Object, unitOfWorkMock.Object, categoryRepositoryMock.Object);

            var action = async () => await usesCases.Handle(input, CancellationToken.None);
            await action.Should().ThrowAsync<EntityValidationException>().WithMessage("Name should not be empty or null");
        }
    }
}
