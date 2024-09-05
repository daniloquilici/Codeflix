using quilici.Codeflix.Catalog.Domain.Entity;
using quilici.Codeflix.Catalog.Domain.SeedWork;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Catalog.Domain.Repository
{
    public interface IGenreRepository : IGenericRepository<Genre>, ISearchableRepository<Genre>
    {
    }
}
