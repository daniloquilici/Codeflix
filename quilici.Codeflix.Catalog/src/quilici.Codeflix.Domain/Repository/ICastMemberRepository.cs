using quilici.Codeflix.Catalog.Domain.Entity;
using quilici.Codeflix.Catalog.Domain.SeedWork;

namespace quilici.Codeflix.Catalog.Domain.Repository;
public interface ICastMemberRepository : IGenericRepository<CastMember>
{
    public Task Delete(CastMember aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<CastMember> Get(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Insert(CastMember aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(CastMember aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
