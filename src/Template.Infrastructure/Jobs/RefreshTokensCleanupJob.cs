using Hangfire;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;

namespace Template.Infrastructure.Jobs {
    public class RefreshTokensCleanupJob {
        public const string JobId = "refresh-tokens-cleanup";
        public static readonly string Schedule = Cron.Weekly();

        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokensCleanupJob(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync() {
            var cutoffDate = DateTime.UtcNow.AddDays(-7);

            await _unitOfWork.RefreshTokens.DeleteExpiredTokensAsync(cutoffDate);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
