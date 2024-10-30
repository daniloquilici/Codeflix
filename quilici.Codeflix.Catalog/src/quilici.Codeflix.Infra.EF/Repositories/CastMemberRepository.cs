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

        if (!string.IsNullOrWhiteSpace(searchInput.Search))
            query = query.Where(x => x.Name.Contains(searchInput.Search));

        var items = await query.Skip(toSkip).Take(searchInput.PerPage).ToListAsync();
        var count = query.Count();
        return new SearchOutput<CastMember>(searchInput.Page, searchInput.PerPage, count, items.AsReadOnly());
    }

    public Task Update(CastMember aggregate, CancellationToken _)
        => Task.FromResult(_castMembers.Update(aggregate));
}
