using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;
using System.Linq;
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

    [Fact(DisplayName = nameof(SearchReturnsItemsAndTotal))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task SearchReturnsItemsAndTotal()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenreList = _fixture.GeteExampleListGenre(10);               
        await dbContext.Genres.AddRangeAsync(exampleGenreList);
        await dbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);
        var searchResult = await genreRepository.Search(searchInput, CancellationToken.None);

        searchResult.Should().NotBeNull();
        searchResult.CurrentPage.Should().Be(searchInput.Page);
        searchResult.PerPage.Should().Be(searchInput.PerPage);
        searchResult.Total.Should().Be(exampleGenreList.Count);
        searchResult.Items.Should().HaveCount(exampleGenreList.Count);

        foreach (var resultItem in searchResult.Items)
        {
            var exampleGenre = exampleGenreList.Find(x => x.Id == resultItem.Id);
            exampleGenre.Should().NotBeNull();
            resultItem.Name.Should().Be(exampleGenre!.Name);
            resultItem.IsActive.Should().Be(exampleGenre.IsActive);
            resultItem.CreatedAt.Should().Be(exampleGenre.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchReturnsRelations))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task SearchReturnsRelations()
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenreList = _fixture.GeteExampleListGenre(10);      
        await dbContext.Genres.AddRangeAsync(exampleGenreList);
        var random = new Random();
        exampleGenreList.ForEach(exampleGenre =>
        {
            var categoriesListRelation = _fixture.GetExampleCategoriesList(random.Next(0, 4));
            if (categoriesListRelation.Count > 0) 
            {
                categoriesListRelation.ForEach(category => exampleGenre.AddCategory(category.Id));
                dbContext.Categories.AddRange(categoriesListRelation);
                var relationsToAdd = categoriesListRelation.Select(category => new GenresCategories(category.Id, exampleGenre.Id)).ToList();
                dbContext.GenresCategories.AddRange(relationsToAdd);                
            }
        });
        await dbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);
        var searchResult = await genreRepository.Search(searchInput, CancellationToken.None);

        searchResult.Should().NotBeNull();
        searchResult.CurrentPage.Should().Be(searchInput.Page);
        searchResult.PerPage.Should().Be(searchInput.PerPage);
        searchResult.Total.Should().Be(exampleGenreList.Count);
        searchResult.Items.Should().HaveCount(exampleGenreList.Count);

        foreach (var resultItem in searchResult.Items)
        {
            var exampleGenre = exampleGenreList.Find(x => x.Id == resultItem.Id);
            exampleGenre.Should().NotBeNull();
            resultItem.Name.Should().Be(exampleGenre!.Name);
            resultItem.IsActive.Should().Be(exampleGenre.IsActive);
            resultItem.CreatedAt.Should().Be(exampleGenre.CreatedAt);
            resultItem.Categories.Should().HaveCount(exampleGenre.Categories.Count);
            resultItem.Categories.Should().BeEquivalentTo(exampleGenre.Categories);
        }
    }

    [Fact(DisplayName = nameof(SearchReturnsEmptyWhenPeristenceIsEmpty))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    public async Task SearchReturnsEmptyWhenPeristenceIsEmpty()
    {
        var actDbContext = _fixture.CreateDbContext();
        var genreRepository = new Repository.GenreRepository(actDbContext);

        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);
        var searchResult = await genreRepository.Search(searchInput, CancellationToken.None);

        searchResult.Should().NotBeNull();
        searchResult.CurrentPage.Should().Be(searchInput.Page);
        searchResult.PerPage.Should().Be(searchInput.PerPage);
        searchResult.Total.Should().Be(0);
        searchResult.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(SearchReturnsPaginated))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnsPaginated(int quantityToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenreList = _fixture.GeteExampleListGenre(quantityToGenerate);
        await dbContext.Genres.AddRangeAsync(exampleGenreList);
        var random = new Random();
        exampleGenreList.ForEach(exampleGenre =>
        {
            var categoriesListRelation = _fixture.GetExampleCategoriesList(random.Next(0, 4));
            if (categoriesListRelation.Count > 0)
            {
                categoriesListRelation.ForEach(category => exampleGenre.AddCategory(category.Id));
                dbContext.Categories.AddRange(categoriesListRelation);
                var relationsToAdd = categoriesListRelation.Select(category => new GenresCategories(category.Id, exampleGenre.Id)).ToList();
                dbContext.GenresCategories.AddRange(relationsToAdd);
            }
        });
        await dbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);
        var searchResult = await genreRepository.Search(searchInput, CancellationToken.None);

        searchResult.Should().NotBeNull();
        searchResult.CurrentPage.Should().Be(searchInput.Page);
        searchResult.PerPage.Should().Be(searchInput.PerPage);
        searchResult.Total.Should().Be(exampleGenreList.Count);
        searchResult.Items.Should().HaveCount(expectedQuantityItems);

        foreach (var resultItem in searchResult.Items)
        {
            var exampleGenre = exampleGenreList.Find(x => x.Id == resultItem.Id);
            exampleGenre.Should().NotBeNull();
            resultItem.Name.Should().Be(exampleGenre!.Name);
            resultItem.IsActive.Should().Be(exampleGenre.IsActive);
            resultItem.CreatedAt.Should().Be(exampleGenre.CreatedAt);
            resultItem.Categories.Should().HaveCount(exampleGenre.Categories.Count);
            resultItem.Categories.Should().BeEquivalentTo(exampleGenre.Categories);
        }
    }

    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByText(string search, int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityItems)
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenreList = _fixture.GetExampleListGenreByNames(new List<string>() { "Action", "Horror", "Horror - Robots", "Horror - Based on Real Facts", "Drama", "Sci-fi IA", "Sci-fi Space", "Sci-fi Robots", "Sci-fi Future" });
        await dbContext.Genres.AddRangeAsync(exampleGenreList);
        var random = new Random();
        exampleGenreList.ForEach(exampleGenre =>
        {
            var categoriesListRelation = _fixture.GetExampleCategoriesList(random.Next(0, 4));
            if (categoriesListRelation.Count > 0)
            {
                categoriesListRelation.ForEach(category => exampleGenre.AddCategory(category.Id));
                dbContext.Categories.AddRange(categoriesListRelation);
                var relationsToAdd = categoriesListRelation.Select(category => new GenresCategories(category.Id, exampleGenre.Id)).ToList();
                dbContext.GenresCategories.AddRange(relationsToAdd);
            }
        });
        await dbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);

        var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);
        var searchResult = await genreRepository.Search(searchInput, CancellationToken.None);

        searchResult.Should().NotBeNull();
        searchResult.CurrentPage.Should().Be(searchInput.Page);
        searchResult.PerPage.Should().Be(searchInput.PerPage);
        searchResult.Total.Should().Be(expectedQuantityItems);
        searchResult.Items.Should().HaveCount(expectedQuantityItemsReturned);

        foreach (var resultItem in searchResult.Items)
        {
            var exampleGenre = exampleGenreList.Find(x => x.Id == resultItem.Id);
            exampleGenre.Should().NotBeNull();
            resultItem.Name.Should().Be(exampleGenre!.Name);
            resultItem.IsActive.Should().Be(exampleGenre.IsActive);
            resultItem.CreatedAt.Should().Be(exampleGenre.CreatedAt);
            resultItem.Categories.Should().HaveCount(exampleGenre.Categories.Count);
            resultItem.Categories.Should().BeEquivalentTo(exampleGenre.Categories);
        }
    }

    [Theory(DisplayName = nameof(SearchOrdered))]
    [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("CreatedAt", "asc")]
    [InlineData("CreatedAt", "desc")]
    [InlineData("", "asc")]
    public async Task SearchOrdered(string orderBy, string order)
    {
        CodeFlixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleGenreList = _fixture.GeteExampleListGenre(10);
        await dbContext.Genres.AddRangeAsync(exampleGenreList);        
        await dbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var genreRepository = new Repository.GenreRepository(actDbContext);
        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);
        
        var output = await genreRepository.Search(searchInput, CancellationToken.None);

        var expectedOrderedList = _fixture.CloneGenreListOrdered(exampleGenreList, orderBy, searchOrder);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleGenreList.Count);
        output.Items.Should().HaveCount(exampleGenreList.Count);
        for (int i = 0; i < expectedOrderedList.Count; i++)
        {
            var expectedItem = expectedOrderedList[i];
            var outputItem = output.Items[i];

            expectedItem.Should().NotBeNull();
            outputItem.Should().NotBeNull();
            outputItem.Id.Should().Be(expectedItem.Id);
            outputItem.Name.Should().Be(expectedItem.Name);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
        }
    }
}
