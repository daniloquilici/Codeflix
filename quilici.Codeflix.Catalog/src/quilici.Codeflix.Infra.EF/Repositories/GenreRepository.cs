using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Domain.Entity;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;
using System.ComponentModel.DataAnnotations;

namespace quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
public class GenreRepository : IGenreRepository
{
    private readonly CodeFlixCatalogDbContext _context;
    private DbSet<Genre> _genres => _context.Set<Genre>();
    private DbSet<GenresCategories> _genresCategories => _context.Set<GenresCategories>();

    public GenreRepository(CodeFlixCatalogDbContext context)
    {
        _context = context;
    }

    public Task Delete(Genre genre, CancellationToken cancellationToken)
    {
        _genresCategories.RemoveRange(
                _genresCategories.Where(x => x.GenreId == genre.Id));
        _genres.Remove(genre);
        return Task.CompletedTask;
    }

    public async Task<Genre> Get(Guid id, CancellationToken cancellationToken)
    {
        var genre = await _genres.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        NotFoundException.ThrowIfNull(genre, $"Genre '{id}' not found.");
        var categoryId = await _genresCategories.Where(x => x.GenreId == genre.Id).Select(x => x.CategoryId).ToListAsync(cancellationToken);
        categoryId.ForEach(genre.AddCategory);
        return genre;
    }

    public async Task Insert(Genre genre, CancellationToken cancellationToken)
    {
        await _genres.AddAsync(genre);
        if (genre.Categories.Count > 0)
        {
            var relations = genre.Categories.Select(categoryId => new GenresCategories(categoryId, genre.Id));
            await _genresCategories.AddRangeAsync(relations);
        }
    }

    public async Task<SearchOutput<Genre>> Search(SearchInput searchInput, CancellationToken cancellationToken)
    {
        var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
        var query = _genres.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchInput.Search))
            query = query.Where(genre => genre.Name.Contains(searchInput.Search));

        var total = await query.CountAsync();

        var genres = await query.Skip(toSkip).Take(searchInput.PerPage).ToListAsync(cancellationToken);
        var genresIds = genres.Select(genre => genre.Id).ToList();
        var relations = await _genresCategories.Where(relation => genresIds.Contains(relation.GenreId)).ToListAsync();
        var relationsByGenreIdGroup = relations.GroupBy(x => x.GenreId).ToList();
        relationsByGenreIdGroup.ForEach(relationGroup =>
        {
            var genre = genres.Find(genre => genre.Id == relationGroup.Key);
            if (genre is null) return;
            relationGroup.ToList().ForEach(relation => genre.AddCategory(relation.CategoryId));
        });
        return new SearchOutput<Genre>(searchInput.Page, searchInput.PerPage, total, genres);
    }

    public async Task Update(Genre genre, CancellationToken cancellationToken)
    {
        _genres.Update(genre);
        _genresCategories.RemoveRange(_genresCategories.Where(x => x.GenreId == genre.Id));
        if (genre.Categories.Count > 0)
        {
            var relations = genre.Categories.Select(categoryId => new GenresCategories(categoryId, genre.Id));
            await _genresCategories.AddRangeAsync(relations);
        }
    }
}
