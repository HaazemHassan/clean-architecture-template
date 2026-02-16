namespace Template.Domain.Contracts.Repositories;

public interface IUnitOfWork
{

    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    bool HasCurrentTransaction();

    IDatabaseExecutionStrategy CreateExecutionStrategy();

}
