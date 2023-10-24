using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Application.Exceptions;
using quilici.Codeflix.Domain.Entity;
using quilici.Codeflix.Domain.Repository;
using quilici.Codeflix.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Infra.Data.EF.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CodeFlixCatalogDbContext _context;
        private DbSet<Category> _categories => _context.Set<Category>();

        public CategoryRepository(CodeFlixCatalogDbContext context)
        {
            _context = context;
        }

        public Task Delete(Category aggregate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categories.FindAsync(new object[] { id }, cancellationToken);
            
            //if (category == null)
            //    throw new NotFoundException($"Category '{id}' not found.");

            NotFoundException.ThrowIfNull(category, $"Category '{id}' not found.");

            return category!;
        }

        public async Task Insert(Category aggregate, CancellationToken cancellationToken)
        {
            await _categories.AddAsync(aggregate, cancellationToken);
        }

        public Task<SearchOutput<Category>> Search(SearchInput searchInput, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Category aggregate, CancellationToken cancellationToken) => Task.FromResult(_categories.Update(aggregate));
    }
}
