using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Domain.Entity;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
public class CastMemberRepository : ICastMemberRepository
{
    private readonly CodeFlixCatalogDbContext _context;
    private DbSet<CastMember> _castMembers => _context.Set<CastMember>();

    public CastMemberRepository(CodeFlixCatalogDbContext context)
    {
        _context = context;
    }

    public Task Delete(CastMember aggregate, CancellationToken cancellationToken)
        => Task.FromResult(_castMembers.Remove(aggregate));

    public async Task<CastMember> Get(Guid id, CancellationToken cancellationToken)
    {
        var castMember = await _castMembers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        NotFoundException.ThrowIfNull(castMember, $"CastMember '{id}' not found.");
        return castMember!;
    }

    public async Task Insert(CastMember aggregate, CancellationToken cancellationToken)
        => await _castMembers.AddAsync(aggregate, cancellationToken);

    public async Task<SearchOutput<CastMember>> Search(SearchInput searchInput, CancellationToken cancellationToken)
    {
        var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
        var query = _castMembers.AsNoTracking();
        query = AddOrderToQuery(query, searchInput.OrderBy, searchInput.Order);

        if (!string.IsNullOrWhiteSpace(searchInput.Search))
            query = query.Where(x => x.Name.Contains(searchInput.Search));

        var total = await query.CountAsync();
        var items = await query.Skip(toSkip).Take(searchInput.PerPage).ToListAsync();
        return new(searchInput.Page, searchInput.PerPage, total, items.AsReadOnly());
    }

    private IQueryable<CastMember> AddOrderToQuery(IQueryable<CastMember> query, string orderProperty, SearchOrder order)
    {
        var orderedeQuery = (orderProperty.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name).ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };

        return orderedeQuery.ThenBy(x => x.CreatedAt);
    }

    public Task Update(CastMember aggregate, CancellationToken _)
        => Task.FromResult(_castMembers.Update(aggregate));
}
