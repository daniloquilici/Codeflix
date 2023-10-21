using quilici.Codeflix.Domain.Entity;
using quilici.Codeflix.Domain.SeedWork;
using quilici.Codeflix.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Domain.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category>, ISearchableRepository<Category>
    {
    }
}
