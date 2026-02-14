namespace Template.Domain.Abstracts.RepositoriesAbstracts {
    public interface IDatabaseTransaction : IAsyncDisposable {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);

    }
}