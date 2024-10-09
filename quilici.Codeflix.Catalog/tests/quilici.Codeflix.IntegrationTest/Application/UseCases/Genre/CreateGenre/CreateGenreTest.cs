using FluentAssertions;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore;


namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.CreateGenre;

[Collection(nameof(CreateGenreTestFixture))]
public class CreateGenreTest
{
    private readonly CreateGenreTestFixture _fixture;

    public CreateGenreTest(CreateGenreTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateGenre))]
    [Trait("Integratrion/Applicatrion", "CreateGenre - Use cases")]
    public async Task CreateGenre()
    {
        CreateGenreInput input = _fixture.GetExampleInput();
        var actDbContext = _fixture.CreateDbContext();
        UseCase.CreateGenre createGente = new UseCase.CreateGenre(new GenreRepository(actDbContext), new UnitOfWork(actDbContext), new CategoryRepository(actDbContext));
        var output = await createGente.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Categories.Should().HaveCount(0);

        var assertDbContext = _fixture.CreateDbContext(true);
        var genreFromDb = await assertDbContext.Genres.FindAsync(output.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Name.Should().Be(input.Name);
        genreFromDb.IsActive.Should().Be(input.IsActive);
    }

    [Fact(DisplayName = nameof(CreateGenreWithCategoriesRelations))]
    [Trait("Integratrion/Applicatrion", "CreateGenre - Use cases")]
    public async Task CreateGenreWithCategoriesRelations()
    {
        List<DomainEntity.Category> exampleCategories = _fixture.GetExampleCategoriesList(5);
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.Categories.AddRangeAsync(exampleCategories);
        await arrangeDbContext.SaveChangesAsync();
        CreateGenreInput input = _fixture.GetExampleInput();
        input.CategoriesIds = exampleCategories.Select(categoty => categoty.Id).ToList();
        var actDbContext = _fixture.CreateDbContext(true);
        UseCase.CreateGenre createGente = new UseCase.CreateGenre(new GenreRepository(actDbContext), new UnitOfWork(actDbContext), new CategoryRepository(actDbContext));
        var output = await createGente.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBe(default(DateTime));
        output.Categories.Should().HaveCount(input.CategoriesIds.Count);
        var relatedCategoriesIdsFromOutput = output.Categories.Select(relation => relation.Id).ToList();
        relatedCategoriesIdsFromOutput.Should().BeEquivalentTo(input.CategoriesIds);
        var assertDbContext = _fixture.CreateDbContext(true);
        var genreFromDb = await assertDbContext.Genres.FindAsync(output.Id);
        genreFromDb.Should().NotBeNull();
        genreFromDb!.Name.Should().Be(input.Name);
        genreFromDb.IsActive.Should().Be(input.IsActive);

        List<GenresCategories> relations = await assertDbContext.GenresCategories.AsNoTracking().Where(x => x.GenreId == output.Id).ToListAsync();
        relations.Should().HaveCount(input.CategoriesIds.Count);
        var categoryIdsRelatedFromDb = relations.Select(relation => relation.CategoryId).ToList();
        categoryIdsRelatedFromDb.Should().BeEquivalentTo(input.CategoriesIds);
    }
}
