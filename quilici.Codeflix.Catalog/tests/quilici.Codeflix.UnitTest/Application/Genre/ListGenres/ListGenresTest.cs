﻿using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.ListGenres
{
    [Collection(nameof(ListGenreTestFixture))]
    public class ListGenresTest
    {
        private readonly ListGenreTestFixture _fixture;

        public ListGenresTest(ListGenreTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(ListGenres))]
        [Trait("Aplication", "ListGenres - Use cases")]
        public async Task ListGenres()
        {
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
            var genreListExample = _fixture.GetExampleGenresList();
            var input = _fixture.GetExampleInput();
            var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
                        currentPage: input.Page,
                        perPage: input.PerPage,
                        items: (IReadOnlyList<DomainEntity.Genre>)genreListExample,
                        total: new Random().Next(50, 200));

            genreRepositoryMock.Setup(x => x.Search(It.IsAny<SearchInput>(), It.IsAny<CancellationToken>())).ReturnsAsync(outputRepositorySearch);

            var useCase = new UseCase.ListGenres(genreRepositoryMock.Object, categoryRepositoryMock.Object);

            UseCase.ListGenresOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
            ((List<GenreModelOutput>)output.Items).ForEach(outpuItem =>
            {
                var repositoryGenre = outputRepositorySearch.Items.FirstOrDefault(y => y.Id == outpuItem.Id);
                repositoryGenre.Should().NotBeNull();
                outpuItem.Should().NotBeNull();
                outpuItem.Name.Should().Be(repositoryGenre!.Name);
                outpuItem.IsActive.Should().Be(repositoryGenre.IsActive);
                outpuItem.CreatedAt.Should().Be(repositoryGenre.CreatedAt);
                outpuItem.Categories.Should().HaveCount(repositoryGenre.Categories.Count);

                foreach (var expectedId in repositoryGenre.Categories)
                {
                    outpuItem.Categories.Should().Contain(x => x.Id == expectedId);
                }
            });

            genreRepositoryMock.Verify(x => x.Search(It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir), It.IsAny<CancellationToken>()), Times.Once);

            var expectedIds = genreListExample.SelectMany(genre => genre.Categories).Distinct().ToList();
            categoryRepositoryMock.Verify(x => x.GetListByIds(
                        It.Is<List<Guid>>(parameterList => parameterList.All(id => expectedIds.Contains(id)
                        && parameterList.Count == expectedIds.Count
                    )), 
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = nameof(ListEmpty))]
        [Trait("Aplication", "ListGenres - Use cases")]
        public async Task ListEmpty()
        {
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
            var input = _fixture.GetExampleInput();
            var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
                        currentPage: input.Page,
                        perPage: input.PerPage,
                        items: (IReadOnlyList<DomainEntity.Genre>)new List<DomainEntity.Genre>(),
                        total: new Random().Next(50, 200));

            genreRepositoryMock.Setup(x => x.Search(It.IsAny<SearchInput>(), It.IsAny<CancellationToken>())).ReturnsAsync(outputRepositorySearch);

            var useCase = new UseCase.ListGenres(genreRepositoryMock.Object, categoryRepositoryMock.Object);

            UseCase.ListGenresOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

            genreRepositoryMock.Verify(x => x.Search(It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.PerPage
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = nameof(ListUsingDefaultInputValues))]
        [Trait("Aplication", "ListGenres - Use cases")]
        public async Task ListUsingDefaultInputValues()
        {
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
            var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
                        currentPage: 1,
                        perPage: 15,
                        items: (IReadOnlyList<DomainEntity.Genre>)new List<DomainEntity.Genre>(),
                        total: 0);

            genreRepositoryMock.Setup(x => x.Search(It.IsAny<SearchInput>(), It.IsAny<CancellationToken>())).ReturnsAsync(outputRepositorySearch);

            var useCase = new UseCase.ListGenres(genreRepositoryMock.Object, categoryRepositoryMock.Object);

            UseCase.ListGenresOutput output = await useCase.Handle(new UseCase.ListGenresInput(), CancellationToken.None);

            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

            genreRepositoryMock.Verify(x => x.Search(It.Is<SearchInput>(searchInput =>
                    searchInput.Page == 1
                    && searchInput.PerPage == 15
                    && searchInput.Search == ""
                    && searchInput.OrderBy == ""
                    && searchInput.Order == SearchOrder.Asc), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
