namespace quilici.Codeflix.Application.Interfaces
{
    public interface IUnitOfWork
    {
<<<<<<< HEAD
        public Task CommitAsync(CancellationToken cancellationToken);
        public Task RollbackAsync(CancellationToken cancellationToken);
=======
        public Task Commit(CancellationToken cancellationToken);
        public Task Rollback(CancellationToken cancellationToken);
>>>>>>> d6f462914faeb68acc03e5802e620ad18dc6d46c
    }
}
