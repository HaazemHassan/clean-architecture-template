using Microsoft.EntityFrameworkCore.Storage;
using Template.Domain.Contracts.Repositories;

namespace Template.Infrastructure.Data.Transactions
{
    public class DatabaseTransaction : IDatabaseTransaction
    {
        private readonly IDbContextTransaction _dbContextTransaction;

        public DatabaseTransaction(IDbContextTransaction efTransaction)
        {
            _dbContextTransaction = efTransaction;
        }

        public async Task CommitAsync(CancellationToken ct) => await _dbContextTransaction.CommitAsync(ct);
        public async Task RollbackAsync(CancellationToken ct) => await _dbContextTransaction.RollbackAsync(ct);
        public async ValueTask DisposeAsync() => await _dbContextTransaction.DisposeAsync();
    }
}
