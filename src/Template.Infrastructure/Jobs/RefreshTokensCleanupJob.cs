using Hangfire;
using Microsoft.Extensions.Logging;
using Template.Domain.Abstracts.RepositoriesAbstracts;

namespace Template.Infrastructure.Jobs {
    public class RefreshTokensCleanupJob {
        public const string JobId = "refresh-tokens-cleanup";
        public static readonly string Schedule = Cron.Weekly();

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefreshTokensCleanupJob> _logger;

        public RefreshTokensCleanupJob(IUnitOfWork unitOfWork, ILogger<RefreshTokensCleanupJob> logger) {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ExecuteAsync() {
            try {
                _logger.LogInformation("RefreshTokensCleanupJob started");
                var cutoffDate = DateTime.UtcNow.AddDays(-7);

                await _unitOfWork.RefreshTokens.DeleteExpiredTokensAsync(cutoffDate);
                var deletedCount = await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("RefreshTokensCleanupJob completed successfully. Deleted {Count} expired tokens (cutoff: {CutoffDate})", deletedCount, cutoffDate);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "RefreshTokensCleanupJob failed with error");
                throw;
            }
        }
    }
}
