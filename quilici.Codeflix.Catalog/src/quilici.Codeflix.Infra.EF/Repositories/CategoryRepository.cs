using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Domain.Entity;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Catalog.Infra.Data.EF.Repositories
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
            query = AddOrderToQuery(query, searchInput.OrderBy, searchInput.Order);

            if (!string.IsNullOrWhiteSpace(searchInput.Search))
                query = query.Where(x => x.Name.Contains(searchInput.Search));

            var total = await query.CountAsync();
            var items = await query.Skip(toSkip).Take(searchInput.PerPage).ToListAsync();
            return new(searchInput.Page, searchInput.PerPage, total, items);
        }

        private IQueryable<Category> AddOrderToQuery(IQueryable<Category> query, string orderProperty, SearchOrder order)
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

        public Task Update(Category aggregate, CancellationToken _) => Task.FromResult(_categories.Update(aggregate));

        public Task<IReadOnlyCollection<Guid>> GetIdsListByIds(List<Guid> ids, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
