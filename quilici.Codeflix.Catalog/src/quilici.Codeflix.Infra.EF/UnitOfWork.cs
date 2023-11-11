using quilici.Codeflix.Application.Interfaces;

namespace quilici.Codeflix.Infra.Data.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CodeFlixCatalogDbContext _context;

<<<<<<< HEAD
        public UnitOfWork(CodeFlixCatalogDbContext codeFlixCatalogDbContext)
        {
            _context = codeFlixCatalogDbContext;
        }

        public Task CommitAsync(CancellationToken cancellationToken)
=======
        public UnitOfWork(CodeFlixCatalogDbContext context)
        {
            _context = context;
        }

        public Task Commit(CancellationToken cancellationToken)
>>>>>>> d6f462914faeb68acc03e5802e620ad18dc6d46c
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

<<<<<<< HEAD
        public Task RollbackAsync(CancellationToken cancellationToken)
=======
        public Task Rollback(CancellationToken cancellationToken)
>>>>>>> d6f462914faeb68acc03e5802e620ad18dc6d46c
        {
            return Task.CompletedTask;
        }
    }
}
