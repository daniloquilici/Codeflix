using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.UpdateGenre;

[Collection(nameof(UpdateGenreTestFixture))]
public class UpdateGenreTest
{
    private readonly UpdateGenreTestFixture _fixture;

    public UpdateGenreTest(UpdateGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(UpdateGenre))]
    [Trait("Integratrion/Applicatrion", "UpdateGenre - Use cases")]
    public async Task UpdateGenre()
    {
        IList<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        DomainEntity.Genre targetGenre = exampleGenres[5];
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.AddRangeAsync(exampleGenres);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        UseCase.UpdateGenre updateGenre = new UseCase.UpdateGenre(new UnitOfWork(actDbContext), new GenreRepository(actDbContext), new CategoryRepository(actDbContext));
        UpdateGenreInput input = new UpdateGenreInput(targetGenre.Id, _fixture.GetValidGenreName(), !targetGenre.IsActive);

        GenreModelOuput output = await updateGenre.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(targetGenre.Id);
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be((bool)input.IsActive!);

        var assertDbContext = _fixture.CreateDbContext(true);
        DomainEntity.Genre? genreFromDb = await assertDbContext.Genres.FindAsync(targetGenre.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Id.Should().Be(targetGenre.Id);
        genreFromDb.Name.Should().Be(input.Name);
        genreFromDb.IsActive.Should().Be((bool)input.IsActive!);
    }

    [Fact(DisplayName = nameof(UpdateGenreWithCategoriesRelations))]
    [Trait("Integratrion/Applicatrion", "UpdateGenre - Use cases")]
    public async Task UpdateGenreWithCategoriesRelations()
    {
        List<DomainEntity.Category> exampleCategories = _fixture.GetExampleCategoriesList(10);
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        DomainEntity.Genre targetGenre = exampleGenres[5];
        var relatedCategories = exampleCategories.GetRange(0, 5);
        var newRelatedCategories = exampleCategories.GetRange(5, 3);
        relatedCategories.ForEach(category => targetGenre.AddCategory(category.Id));
        List<GenresCategories> relations = targetGenre.Categories.Select(category => new GenresCategories(category, targetGenre.Id)).ToList();
        
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.AddRangeAsync(exampleGenres);
        await arrangeDbContext.AddRangeAsync(exampleCategories);
        await arrangeDbContext.AddRangeAsync(relations);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        UseCase.UpdateGenre updateGenre = new UseCase.UpdateGenre(new UnitOfWork(actDbContext), new GenreRepository(actDbContext), new CategoryRepository(actDbContext));
        UpdateGenreInput input = new UpdateGenreInput(targetGenre.Id, _fixture.GetValidGenreName(), !targetGenre.IsActive, newRelatedCategories.Select(category => category.Id).ToList());

        GenreModelOuput output = await updateGenre.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(targetGenre.Id);
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be((bool)input.IsActive!);
        output.Categories.Should().HaveCount(newRelatedCategories.Count);
        var relatedCategoryIdsFromOutput = output.Categories.Select(category => category.Id).ToList();
        relatedCategoryIdsFromOutput.Should().NotBeNullOrEmpty();
        relatedCategoryIdsFromOutput.Should().BeEquivalentTo(input.CategoriesIds);

        var assertDbContext = _fixture.CreateDbContext(true);
        DomainEntity.Genre? genreFromDb = await assertDbContext.Genres.FindAsync(targetGenre.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Id.Should().Be(targetGenre.Id);
        genreFromDb.Name.Should().Be(input.Name);
        genreFromDb.IsActive.Should().Be((bool)input.IsActive!);
        var relationsFromDb = await assertDbContext.GenresCategories.AsNoTracking()
            .Where(relation => relation.GenreId == input.Id)
            .Select(relation => relation.CategoryId)
            .ToListAsync();
        relationsFromDb.Should().NotBeNullOrEmpty();
        relationsFromDb.Should().BeEquivalentTo(input.CategoriesIds);
    }

    [Fact(DisplayName = nameof(UpdateGenreThrowsWhenCategoryDoesntExist))]
    [Trait("Integratrion/Applicatrion", "UpdateGenre - Use cases")]
    public async Task UpdateGenreThrowsWhenCategoryDoesntExist()
    {
        List<DomainEntity.Category> exampleCategories = _fixture.GetExampleCategoriesList(10);
        List<DomainEntity.Genre> exampleGenres = _fixture.GetExampleListGenre(10);
        DomainEntity.Genre targetGenre = exampleGenres[5];
        var relatedCategories = exampleCategories.GetRange(0, 5);
        var newRelatedCategories = exampleCategories.GetRange(5, 3);
        relatedCategories.ForEach(category => targetGenre.AddCategory(category.Id));
        List<GenresCategories> relations = targetGenre.Categories.Select(category => new GenresCategories(category, targetGenre.Id)).ToList();

        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.AddRangeAsync(exampleGenres);
        await arrangeDbContext.AddRangeAsync(exampleCategories);
        await arrangeDbContext.AddRangeAsync(relations);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        UseCase.UpdateGenre updateGenre = new UseCase.UpdateGenre(new UnitOfWork(actDbContext), new GenreRepository(actDbContext), new CategoryRepository(actDbContext));
        var categoryIdsToRelate = newRelatedCategories.Select(category => category.Id).ToList();
        Guid invalidCategoryId = Guid.NewGuid();
        categoryIdsToRelate.Add(invalidCategoryId);
        UpdateGenreInput input = new UpdateGenreInput(targetGenre.Id, _fixture.GetValidGenreName(), !targetGenre.IsActive, categoryIdsToRelate);

        Func<Task<GenreModelOuput>> action = async () => await updateGenre.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage($"Related category id(s) not found: {invalidCategoryId}");
    }
}
