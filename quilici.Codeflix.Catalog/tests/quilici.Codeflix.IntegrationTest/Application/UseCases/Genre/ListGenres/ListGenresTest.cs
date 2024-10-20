﻿using FluentAssertions;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.ListGenres;

[Collection(nameof(ListGenresTestFixture))]
public class ListGenresTest
{
    private readonly ListGenresTestFixture _fixture;

    public ListGenresTest(ListGenresTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListGenres))]
    [Trait("Integration/Application", "ListGenres - UseCases")]
    public async void ListGenres()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre();
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.Genres.AddRangeAsync(exampleGenres);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContex = _fixture.CreateDbContext(true);
        var useCase = new UseCase.ListGenres(new GenreRepository(actDbContex), new CategoryRepository(actDbContex));
        var input = new UseCase.ListGenresInput(1, 20);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleGenres.Count);
        output.Items.Should().HaveCount(exampleGenres.Count);
        output.Items.ToList().ForEach(outputItem =>
        {
            DomainEntity.Genre? exampleItem = exampleGenres.Find(example => example.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
        });
    }

    [Fact(DisplayName = nameof(ListGenresReturnsEmptyWhenPersistenceIsEmpty))]
    [Trait("Integration/Application", "ListGenres - UseCases")]
    public async void ListGenresReturnsEmptyWhenPersistenceIsEmpty()
    {
        var actDbContext = _fixture.CreateDbContext();
        var useCase = new UseCase.ListGenres(new GenreRepository(actDbContext), new CategoryRepository(actDbContext));
        var input = new UseCase.ListGenresInput(1, 20);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(ListGenresVerifyRelations))]
    [Trait("Integration/Application", "ListGenres - UseCases")]
    public async void ListGenresVerifyRelations()
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre();
        List<DomainEntity.Category> exampleCategories = _fixture.GetExampleCategoriesList();
        Random random = new Random();
        exampleGenres.ForEach(genres =>
        {
            int relationsCount = random.Next(0, 3);
            for (int i = 0; i < relationsCount; i++)
            {
                var selected = exampleCategories[random.Next(0, exampleCategories.Count - 1)];
                if (!genres.Categories.Contains(selected.Id))
                    genres.AddCategory(selected.Id);
            }
        });
        var genresCategories = new List<GenresCategories>();
        exampleGenres.ForEach(genre =>
            genre.Categories.ToList().ForEach(categoryId => genresCategories.Add(new GenresCategories(categoryId, genre.Id)))
            );
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.Genres.AddRangeAsync(exampleGenres);
        await arrangeDbContext.Categories.AddRangeAsync(exampleCategories);
        await arrangeDbContext.GenresCategories.AddRangeAsync(genresCategories);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var useCase = new UseCase.ListGenres(new GenreRepository(actDbContext), new CategoryRepository(actDbContext));
        var input = new UseCase.ListGenresInput(1, 20);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleGenres.Count);
        output.Items.Should().HaveCount(exampleGenres.Count);
        output.Items.ToList().ForEach(outputItem =>
        {
            DomainEntity.Genre? exampleItem = exampleGenres.Find(example => example.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);

            var outputItemCategoryIds = outputItem.Categories.Select(x => x.Id).ToList();
            outputItemCategoryIds.Should().BeEquivalentTo(exampleItem.Categories);

            outputItem.Categories.ToList().ForEach(outputCategory => 
            {
                var exampleCategory = exampleCategories.Find(x => x.Id == outputCategory.Id);
                exampleCategory.Should().NotBeNull();
                outputCategory.Name.Should().Be(exampleCategory!.Name);
            });
        });
    }

    [Theory(DisplayName = nameof(ListGenresPaginated))]
    [Trait("Integration/Application", "ListGenres - UseCases")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async void ListGenresPaginated(int quantityToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(quantityToGenerate);
        List<DomainEntity.Category> exampleCategories = _fixture.GetExampleCategoriesList();
        Random random = new Random();
        exampleGenres.ForEach(genres =>
        {
            int relationsCount = random.Next(0, 3);
            for (int i = 0; i < relationsCount; i++)
            {
                var selected = exampleCategories[random.Next(0, exampleCategories.Count - 1)];
                if (!genres.Categories.Contains(selected.Id))
                    genres.AddCategory(selected.Id);
            }
        });
        var genresCategories = new List<GenresCategories>();
        exampleGenres.ForEach(genre =>
            genre.Categories.ToList().ForEach(categoryId => genresCategories.Add(new GenresCategories(categoryId, genre.Id)))
            );
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.Genres.AddRangeAsync(exampleGenres);
        await arrangeDbContext.Categories.AddRangeAsync(exampleCategories);
        await arrangeDbContext.GenresCategories.AddRangeAsync(genresCategories);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var useCase = new UseCase.ListGenres(new GenreRepository(actDbContext), new CategoryRepository(actDbContext));
        var input = new UseCase.ListGenresInput(page, perPage);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleGenres.Count);
        output.Items.Should().HaveCount(expectedQuantityItems);
        output.Items.ToList().ForEach(outputItem =>
        {
            DomainEntity.Genre? exampleItem = exampleGenres.Find(example => example.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);

            var outputItemCategoryIds = outputItem.Categories.Select(x => x.Id).ToList();
            outputItemCategoryIds.Should().BeEquivalentTo(exampleItem.Categories);

            outputItem.Categories.ToList().ForEach(outputCategory =>
            {
                var exampleCategory = exampleCategories.Find(x => x.Id == outputCategory.Id);
                exampleCategory.Should().NotBeNull();
                outputCategory.Name.Should().Be(exampleCategory!.Name);
            });
        });
    }

    [Theory(DisplayName = nameof(SeachByText))]
    [Trait("Integration/Application", "ListGenres - UseCases")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async void SeachByText(string search, int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityItems)
    {
        var exampleGenres = _fixture.GetExampleListGenreByNames(new List<string>() { "Action", "Horror", "Horror - Robots", "Horror - Based on Real Facts", "Drama", "Sci-fi IA", "Sci-fi Space", "Sci-fi Robots", "Sci-fi Future" });        
        List<DomainEntity.Category> exampleCategories = _fixture.GetExampleCategoriesList(10);
        Random random = new Random();
        exampleGenres.ForEach(genres =>
        {
            int relationsCount = random.Next(0, 3);
            for (int i = 0; i < relationsCount; i++)
            {
                var selected = exampleCategories[random.Next(0, exampleCategories.Count - 1)];
                if (!genres.Categories.Contains(selected.Id))
                    genres.AddCategory(selected.Id);
            }
        });
        var genresCategories = new List<GenresCategories>();
        exampleGenres.ForEach(genre =>
            genre.Categories.ToList().ForEach(categoryId => genresCategories.Add(new GenresCategories(categoryId, genre.Id)))
            );
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.Genres.AddRangeAsync(exampleGenres);
        await arrangeDbContext.Categories.AddRangeAsync(exampleCategories);
        await arrangeDbContext.GenresCategories.AddRangeAsync(genresCategories);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var useCase = new UseCase.ListGenres(new GenreRepository(actDbContext), new CategoryRepository(actDbContext));
        var input = new UseCase.ListGenresInput(page, perPage, search);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);
        output.Items.ToList().ForEach(outputItem =>
        {
            DomainEntity.Genre? exampleItem = exampleGenres.Find(example => example.Id == outputItem.Id);
            outputItem.Name.Should().Contain(search);
            exampleItem.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);

            var outputItemCategoryIds = outputItem.Categories.Select(x => x.Id).ToList();
            outputItemCategoryIds.Should().BeEquivalentTo(exampleItem.Categories);

            outputItem.Categories.ToList().ForEach(outputCategory =>
            {
                var exampleCategory = exampleCategories.Find(x => x.Id == outputCategory.Id);
                exampleCategory.Should().NotBeNull();
                outputCategory.Name.Should().Be(exampleCategory!.Name);
            });
        });
    }

    [Theory(DisplayName = nameof(Ordered))]
    [Trait("Integration/Application", "ListGenres - UseCases")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("CreatedAt", "asc")]
    [InlineData("CreatedAt", "desc")]
    [InlineData("", "asc")]
    public async void Ordered(string orderBy, string order)
    {
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        List<DomainEntity.Category> exampleCategories = _fixture.GetExampleCategoriesList();
        Random random = new Random();
        exampleGenres.ForEach(genres =>
        {
            int relationsCount = random.Next(0, 3);
            for (int i = 0; i < relationsCount; i++)
            {
                var selected = exampleCategories[random.Next(0, exampleCategories.Count - 1)];
                if (!genres.Categories.Contains(selected.Id))
                    genres.AddCategory(selected.Id);
            }
        });
        var genresCategories = new List<GenresCategories>();
        exampleGenres.ForEach(genre =>
            genre.Categories.ToList().ForEach(categoryId => genresCategories.Add(new GenresCategories(categoryId, genre.Id)))
            );
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.Genres.AddRangeAsync(exampleGenres);
        await arrangeDbContext.Categories.AddRangeAsync(exampleCategories);
        await arrangeDbContext.GenresCategories.AddRangeAsync(genresCategories);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var useCase = new UseCase.ListGenres(new GenreRepository(actDbContext), new CategoryRepository(actDbContext));

        var orderEnum = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var input = new UseCase.ListGenresInput(1, 20, "", orderBy, orderEnum);
        var output = await useCase.Handle(input, CancellationToken.None);

        var expectedOrderedList = _fixture.CloneGenreListOrdered(exampleGenres, orderBy, orderEnum);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleGenres.Count);
        output.Items.Should().HaveCount(exampleGenres.Count);

        for (int i = 0; i < expectedOrderedList.Count; i++)
        {
            var expectedItem = expectedOrderedList[i];
            var outputItem = output.Items[i];

            expectedItem.Should().NotBeNull();
            outputItem.Name.Should().Be(expectedItem!.Name);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            var outputItemCategoryIds = outputItem.Categories.Select(x => x.Id).ToList();
            outputItemCategoryIds.Should().BeEquivalentTo(expectedItem.Categories);

            outputItem.Categories.ToList().ForEach(outputCategory =>
            {
                var exampleCategory = exampleCategories.Find(x => x.Id == outputCategory.Id);
                exampleCategory.Should().NotBeNull();
                outputCategory.Name.Should().Be(exampleCategory!.Name);
            });
        }
    }
}
