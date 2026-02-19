using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Template.Domain.Contracts.Repositories;
using Template.Infrastructure.Data.Transactions;

namespace Template.Infrastructure.Data;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private IDbContextTransaction? _transaction;

    private IUserRepository? _userRepository;
    private IRefreshTokenRepository? _refreshTokenRepository;

    public UnitOfWork(AppDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public IUserRepository Users => _serviceProvider.GetRequiredService<IUserRepository>();
    public IRefreshTokenRepository RefreshTokens => _serviceProvider.GetRequiredService<IRefreshTokenRepository>();

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new DatabaseTransaction(_transaction);
    }

    public bool HasCurrentTransaction()
    {
        return _context.Database.CurrentTransaction != null;
    }

    public IDatabaseExecutionStrategy CreateExecutionStrategy()
    {
        var efStrategy = _context.Database.CreateExecutionStrategy();
        return new DatabaseExecutionStrategy(efStrategy);
    }
}
