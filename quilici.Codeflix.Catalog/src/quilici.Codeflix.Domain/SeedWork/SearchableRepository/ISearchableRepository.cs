using quilici.Codeflix.Catalog.Domain.SeedWork;

namespace quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository
{
    public interface ISearchableRepository<TAggregate> where TAggregate : AggregateRoot
    {
        Task<SearchOutput<TAggregate>> Search(SearchInput searchInput, CancellationToken cancellationToken);
    }
}
