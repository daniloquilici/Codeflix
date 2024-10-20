using quilici.Codeflix.Catalog.Infra.Data.EF;
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
}
