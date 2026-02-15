using Microsoft.EntityFrameworkCore.Storage;
using Template.Domain.Abstracts.RepositoriesAbstracts;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.Repositories;

internal class UnitOfWork : IUnitOfWork {
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    private IUserRepository? _userRepository;
    private IRefreshTokenRepository? _refreshTokenRepository;

    public UnitOfWork(AppDbContext context) {
        _context = context;
    }

    public IUserRepository Users => _userRepository ??= new UserRepository(_context);
    public IRefreshTokenRepository RefreshTokens => _refreshTokenRepository ??= new RefreshTokenRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new DatabaseTransaction(_transaction);
    }


}
