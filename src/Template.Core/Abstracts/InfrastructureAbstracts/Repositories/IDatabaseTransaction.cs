namespace Template.Core.Abstracts.InfrastructureAbstracts.Repositories {
    public interface IDatabaseTransaction : IAsyncDisposable {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);

    }
}