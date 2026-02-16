using Microsoft.EntityFrameworkCore;
using Template.Domain.Contracts.Repositories;

namespace Template.Infrastructure.Repositories;

internal class DatabaseExecutionStrategy : IDatabaseExecutionStrategy
{
    private readonly Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy _efExecutionStrategy;

    public DatabaseExecutionStrategy(Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy efExecutionStrategy)
    {
        _efExecutionStrategy = efExecutionStrategy;
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        Func<Task<TResult>> operation,
        CancellationToken cancellationToken = default)
    {
        return await ExecutionStrategyExtensions.ExecuteAsync(
            _efExecutionStrategy,
            (ct) => operation(),
            cancellationToken);
    }

    public async Task ExecuteAsync(
        Func<Task> operation,
        CancellationToken cancellationToken = default)
    {
        await ExecutionStrategyExtensions.ExecuteAsync(
            _efExecutionStrategy,
            (ct) => operation(),
            cancellationToken);
    }
}
