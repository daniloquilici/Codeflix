using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.Common;
public class CastMemberPesistence
{
    private readonly CodeFlixCatalogDbContext _context;

    public CastMemberPesistence(CodeFlixCatalogDbContext context)
        => _context = context;

    public async Task InsertList(IList<DomainEntity.CastMember> castMember)
    {
        await _context.CastMembers.AddRangeAsync(castMember);
        await _context.SaveChangesAsync();
    }

    public async Task<DomainEntity.CastMember?> GetById(Guid id)
    {
        return await _context.CastMembers.AsNoTracking().FirstOrDefaultAsync(castMember => castMember.Id == id);
    }
}
