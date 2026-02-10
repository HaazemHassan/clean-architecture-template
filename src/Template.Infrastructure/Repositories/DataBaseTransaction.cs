using Microsoft.EntityFrameworkCore.Storage;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;

namespace Template.Infrastructure.Repositories {
    public class DataBaseTransaction : IDatabaseTransaction {
        private readonly IDbContextTransaction _dbContextTransaction;

        public DataBaseTransaction(IDbContextTransaction efTransaction) {
            _dbContextTransaction = efTransaction;
        }

        public async Task CommitAsync(CancellationToken ct) => await _dbContextTransaction.CommitAsync(ct);
        public async Task RollbackAsync(CancellationToken ct) => await _dbContextTransaction.RollbackAsync(ct);
        public async ValueTask DisposeAsync() => await _dbContextTransaction.DisposeAsync();
    }
}
