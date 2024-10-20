using quilici.Codeflix.Catalog.Infra.Data.EF;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.Common;
public class GenrePersistence
{
    private readonly CodeFlixCatalogDbContext _context;

    public GenrePersistence(CodeFlixCatalogDbContext context)
        => _context = context;
}
