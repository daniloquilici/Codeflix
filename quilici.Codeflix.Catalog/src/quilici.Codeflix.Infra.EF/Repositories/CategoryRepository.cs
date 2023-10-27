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

        public Task Delete(Category aggregate, CancellationToken _)
        {
            return Task.FromResult(_categories.Remove(aggregate));
        }

        public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
        {            
            var category = await _categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            //var category = await _categories.FindAsync(new object[] { id }, cancellationToken);

            //if (category == null)
            //    throw new NotFoundException($"Category '{id}' not found.");

            NotFoundException.ThrowIfNull(category, $"Category '{id}' not found.");

            return category!;
        }

        public async Task Insert(Category aggregate, CancellationToken cancellationToken)
        {
            await _categories.AddAsync(aggregate, cancellationToken);
        }

        public async Task<SearchOutput<Category>> Search(SearchInput searchInput, CancellationToken cancellationToken)
        {
            var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
            var query = _categories.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(searchInput.Search))
                query = query.Where(x => x.Name.Contains(searchInput.Search));

            var total = await query.CountAsync();
            var items = await query.Skip(toSkip).Take(searchInput.PerPage).ToListAsync();
            return new (searchInput.Page, searchInput.PerPage, total, items);
        }

        public Task Update(Category aggregate, CancellationToken _) => Task.FromResult(_categories.Update(aggregate));
    }
}
