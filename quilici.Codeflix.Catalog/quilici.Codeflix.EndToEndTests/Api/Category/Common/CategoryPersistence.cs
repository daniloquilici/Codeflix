using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Infra.Data.EF;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.Common
{
    public class CategoryPersistence
    {
        private readonly CodeFlixCatalogDbContext _context;

        public CategoryPersistence(CodeFlixCatalogDbContext context)
            => _context = context;

        public async Task<Domain.Entity.Category?> GetById(Guid id)
            => await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task InsertList(IList<Domain.Entity.Category> categories) 
        {   
            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }
    }
}
