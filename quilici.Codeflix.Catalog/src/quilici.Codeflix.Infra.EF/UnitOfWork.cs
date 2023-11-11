using quilici.Codeflix.Application.Interfaces;

namespace quilici.Codeflix.Infra.Data.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CodeFlixCatalogDbContext _context;

        public UnitOfWork(CodeFlixCatalogDbContext codeFlixCatalogDbContext)
        {
            _context = codeFlixCatalogDbContext;
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
