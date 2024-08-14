using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Infra.Data.EF;
using DomainEntity = quilici.Codeflix.Domain.Entity;

namespace quilici.Codeflix.EndToEndTests.Api.Category.Common
{
    public class CategoryPersistence
    {
        private readonly CodeFlixCatalogDbContext _context;

        public CategoryPersistence(CodeFlixCatalogDbContext context)
            => _context = context;

        public async Task<DomainEntity.Category?> GetById(Guid id)
            => await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
}
