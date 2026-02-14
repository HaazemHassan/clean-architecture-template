namespace Template.Domain.Abstracts.RepositoriesAbstracts;

public interface IUnitOfWork {

    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

}
