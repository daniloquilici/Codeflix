using quilici.Codeflix.Catalog.Domain.Entity;
using quilici.Codeflix.Catalog.Domain.SeedWork;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Catalog.Domain.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category>, ISearchableRepository<Category>
    {
        public Task<IReadOnlyCollection<Guid>> GetIdsListByIds(List<Guid> ids, CancellationToken cancellationToken);
    }
}
