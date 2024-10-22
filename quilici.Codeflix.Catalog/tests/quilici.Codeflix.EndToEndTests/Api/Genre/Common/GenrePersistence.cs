using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Models;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.Common;
public class GenrePersistence
{
    private readonly CodeFlixCatalogDbContext _context;

    public GenrePersistence(CodeFlixCatalogDbContext context)
        => _context = context;

    public async Task InsertList(IList<DomainEntity.Genre> genres)
    {
        await _context.Genres.AddRangeAsync(genres);
        await _context.SaveChangesAsync();
    }

    public async Task InsertGenresCategoriesRelationsList(IList<GenresCategories> genresCategories)
    {
        await _context.GenresCategories.AddRangeAsync(genresCategories);
        await _context.SaveChangesAsync();
    }

    public async Task<DomainEntity.Genre?> GetById(Guid id)
    {
        return await _context.Genres.AsNoTracking().FirstOrDefaultAsync(genre => genre.Id == id);
    }

    internal async Task<List<GenresCategories>> GetGenresCategoriesRelationsByGenreId(Guid id)
    {
        return await _context.GenresCategories.AsNoTracking().Where(relation => relation.GenreId == id).ToListAsync();
    }
}
