﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;
using Xunit;
using Repository = quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;

namespace quilici.Codeflix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.GenreRepository;
[Collection(nameof(GenreRepositoryTestFixture))]
public class GenreRepositoryTest
{
    private readonly GenreRepositoryTestFixture _fixture;
    public GenreRepositoryTest(GenreRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task Insert()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenre = _fixture.GetExampleGenre();
        var categoriesListExample = _fixture.GetExampleCategoriesList(3);
        categoriesListExample.ForEach(x => exampleGenre.AddCategory(x.Id));
        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        var genreRepository = new Repository.GenreRepository(dbContext);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        await genreRepository.Insert(exampleGenre, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var assertesDBContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertesDBContext.Genres.FindAsync(exampleGenre.Id);

        dbGenre.Should().NotBeNull();
        dbGenre!.Name.Should().Be(exampleGenre.Name);
        dbGenre.IsActive.Should().Be(exampleGenre.IsActive);
        dbGenre.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        var genreCategoriesRelations = await assertesDBContext.GenresCategories.Where(x => x.GenreId == exampleGenre.Id).ToListAsync();
        genreCategoriesRelations.Should().HaveCount(categoriesListExample.Count);
        genreCategoriesRelations.ForEach(relation =>
        {
            var expectedCategory = categoriesListExample.FirstOrDefault(x => x.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task Get()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenre = _fixture.GetExampleGenre();
        var categoriesListExample = _fixture.GetExampleCategoriesList(3);
        categoriesListExample.ForEach(x => exampleGenre.AddCategory(x.Id));
        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        await dbContext.Genres.AddAsync(exampleGenre);
        foreach (var categoryId in exampleGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, exampleGenre.Id);
            await dbContext.GenresCategories.AddAsync(relation);
        }
        await dbContext.SaveChangesAsync();

        var genreRepository = new Repository.GenreRepository(_fixture.CreateDbContext(true));

        var genreFromRepository = await genreRepository.Get(exampleGenre.Id, CancellationToken.None);
        genreFromRepository.Should().NotBeNull();
        genreFromRepository!.Name.Should().Be(exampleGenre.Name);
        genreFromRepository.IsActive.Should().Be(exampleGenre.IsActive);
        genreFromRepository.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        genreFromRepository.Categories.Should().HaveCount(categoriesListExample.Count);
        foreach (var categoryId in genreFromRepository.Categories)
        {
            var expectedCategory = categoriesListExample.FirstOrDefault(x => x.Id == categoryId);
            expectedCategory.Should().NotBeNull();
        }
    }

    [Fact(DisplayName = nameof(GetThrowWhenNotFound))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task GetThrowWhenNotFound()
    {
        var exampleNotFoundGuid = Guid.NewGuid();
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenre = _fixture.GetExampleGenre();
        var categoriesListExample = _fixture.GetExampleCategoriesList(3);
        categoriesListExample.ForEach(x => exampleGenre.AddCategory(x.Id));
        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        await dbContext.Genres.AddAsync(exampleGenre);
        foreach (var categoryId in exampleGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, exampleGenre.Id);
            await dbContext.GenresCategories.AddAsync(relation);
        }
        await dbContext.SaveChangesAsync();

        var genreRepository = new Repository.GenreRepository(_fixture.CreateDbContext(true));

        var action = async () => await genreRepository.Get(exampleNotFoundGuid, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{exampleNotFoundGuid}' not found.");
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task Delete()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenre = _fixture.GetExampleGenre();
        var categoriesListExample = _fixture.GetExampleCategoriesList(3);
        categoriesListExample.ForEach(x => exampleGenre.AddCategory(x.Id));
        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        await dbContext.Genres.AddAsync(exampleGenre);
        foreach (var categoryId in exampleGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, exampleGenre.Id);
            await dbContext.GenresCategories.AddAsync(relation);
        }
        await dbContext.SaveChangesAsync();
        var repositoryDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(repositoryDbContext);
        await genreRepository.Delete(exampleGenre, CancellationToken.None);
        await repositoryDbContext.SaveChangesAsync();

        var assertsDbContext = _fixture.CreateDbContext(true);
        var dbGenre = assertsDbContext.Genres.AsNoTracking().FirstOrDefault(x => x.Id == exampleGenre.Id);
        dbGenre.Should().BeNull();
        var categoriesIdList = await assertsDbContext.GenresCategories.AsNoTracking().Where(x => x.GenreId == exampleGenre.Id).Select(x => x.CategoryId).ToListAsync();
        categoriesIdList.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task Update()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenre = _fixture.GetExampleGenre();
        var categoriesListExample = _fixture.GetExampleCategoriesList(3);
        categoriesListExample.ForEach(x => exampleGenre.AddCategory(x.Id));
        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        await dbContext.Genres.AddAsync(exampleGenre);
        foreach (var categoryId in exampleGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, exampleGenre.Id);
            await dbContext.GenresCategories.AddAsync(relation);
        }
        await dbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        exampleGenre.Update(_fixture.GetValidGenreName());
        if (exampleGenre.IsActive)
            exampleGenre.Deactivate();
        else
            exampleGenre.Activate();

        await genreRepository.Update(exampleGenre, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        var assertContext = _fixture.CreateDbContext(true);
        var genreFromDb = await assertContext.Genres.FindAsync(exampleGenre.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Name.Should().Be(exampleGenre.Name);
        genreFromDb.IsActive.Should().Be(exampleGenre.IsActive);
        genreFromDb.CreatedAt.Should().Be(exampleGenre.CreatedAt);

        var assertsDBContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertsDBContext.Genres.FindAsync(exampleGenre.Id);

        dbGenre.Should().NotBeNull();
        dbGenre!.Name.Should().Be(exampleGenre.Name);
        dbGenre.IsActive.Should().Be(exampleGenre.IsActive);
        dbGenre.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        var genreCategoriesRelations = await assertsDBContext.GenresCategories.Where(x => x.GenreId == exampleGenre.Id).ToListAsync();
        genreCategoriesRelations.Should().HaveCount(categoriesListExample.Count);
        genreCategoriesRelations.ForEach(relation =>
        {
            var expectedCategory = categoriesListExample.FirstOrDefault(x => x.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }

    [Fact(DisplayName = nameof(UpdateRemovingRelations))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task UpdateRemovingRelations()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenre = _fixture.GetExampleGenre();
        var categoriesListExample = _fixture.GetExampleCategoriesList(3);
        categoriesListExample.ForEach(x => exampleGenre.AddCategory(x.Id));
        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        await dbContext.Genres.AddAsync(exampleGenre);
        foreach (var categoryId in exampleGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, exampleGenre.Id);
            await dbContext.GenresCategories.AddAsync(relation);
        }
        await dbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        exampleGenre.Update(_fixture.GetValidGenreName());
        if (exampleGenre.IsActive)
            exampleGenre.Deactivate();
        else
            exampleGenre.Activate();

        exampleGenre.RemoveAllCategories();
        await genreRepository.Update(exampleGenre, CancellationToken.None);        
        await actDbContext.SaveChangesAsync();

        var assertsDBContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertsDBContext.Genres.FindAsync(exampleGenre.Id);

        dbGenre.Should().NotBeNull();
        dbGenre!.Name.Should().Be(exampleGenre.Name);
        dbGenre.IsActive.Should().Be(exampleGenre.IsActive);
        dbGenre.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        var genreCategoriesRelations = await assertsDBContext.GenresCategories.Where(x => x.GenreId == exampleGenre.Id).ToListAsync();
        genreCategoriesRelations.Should().HaveCount(0);        
    }

    [Fact(DisplayName = nameof(UpdateReplacingRelations))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task UpdateReplacingRelations()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenre = _fixture.GetExampleGenre();
        var categoriesListExample = _fixture.GetExampleCategoriesList(3);
        var updateCategoriesListExample = _fixture.GetExampleCategoriesList(2);
        categoriesListExample.ForEach(x => exampleGenre.AddCategory(x.Id));
        await dbContext.Categories.AddRangeAsync(categoriesListExample);
        await dbContext.Categories.AddRangeAsync(updateCategoriesListExample);
        await dbContext.Genres.AddAsync(exampleGenre);
        foreach (var categoryId in exampleGenre.Categories)
        {
            var relation = new GenresCategories(categoryId, exampleGenre.Id);
            await dbContext.GenresCategories.AddAsync(relation);
        }
        await dbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        exampleGenre.Update(_fixture.GetValidGenreName());
        if (exampleGenre.IsActive)
            exampleGenre.Deactivate();
        else
            exampleGenre.Activate();

        exampleGenre.RemoveAllCategories();
        updateCategoriesListExample.ForEach(category => exampleGenre.AddCategory(category.Id));
        await genreRepository.Update(exampleGenre, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        var assertsDBContext = _fixture.CreateDbContext(true);
        var dbGenre = await assertsDBContext.Genres.FindAsync(exampleGenre.Id);

        dbGenre.Should().NotBeNull();
        dbGenre!.Name.Should().Be(exampleGenre.Name);
        dbGenre.IsActive.Should().Be(exampleGenre.IsActive);
        dbGenre.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        var genreCategoriesRelations = await assertsDBContext.GenresCategories.Where(x => x.GenreId == exampleGenre.Id).ToListAsync();
        genreCategoriesRelations.Should().HaveCount(updateCategoriesListExample.Count);
        genreCategoriesRelations.ForEach(relation =>
        {
            var expectedCategory = updateCategoriesListExample.FirstOrDefault(x => x.Id == relation.CategoryId);
            expectedCategory.Should().NotBeNull();
        });
    }
}
