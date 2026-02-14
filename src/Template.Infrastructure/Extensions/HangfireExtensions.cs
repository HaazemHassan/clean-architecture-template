
using Hangfire;
using Template.Infrastructure.Jobs;

namespace Template.Infrastructure.Extensions {
    public static class HangfireExtensions {
        public static void RegisterRecurringJobs(this IRecurringJobManager manager) {
            manager.AddOrUpdate<RefreshTokensCleanupJob>(
                RefreshTokensCleanupJob.JobId,
                job => job.ExecuteAsync(),
                RefreshTokensCleanupJob.Schedule
            );

        }
    }
}

