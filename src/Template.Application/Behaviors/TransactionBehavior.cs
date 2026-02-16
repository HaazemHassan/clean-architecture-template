using MediatR;
using Microsoft.Extensions.Logging;
using Template.Application.Contracts;
using Template.Domain.Contracts.Repositories;

namespace Template.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        public TransactionBehavior(
            ILogger<TransactionBehavior<TRequest, TResponse>> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is not ITransactionalRequest)
                return await next();

            if (_unitOfWork.HasCurrentTransaction())
                return await next();

            var requestName = typeof(TRequest).Name;
            var strategy = _unitOfWork.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _unitOfWork
                    .BeginTransactionAsync(cancellationToken);

                _logger.LogInformation(
                    "Begin transaction for {RequestName}",
                    requestName);

                try
                {
                    var response = await next();

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    _logger.LogInformation(
                        "Committed transaction for {RequestName}",
                        requestName);

                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Rolling back transaction for {RequestName}",
                        requestName);

                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }, cancellationToken);
        }
    }
}
