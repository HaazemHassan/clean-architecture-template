
using Hangfire;
using Template.Infrastructure.BackgroundJobs.Jobs;

namespace Template.Infrastructure.BackgroundJobs
{
    public static class HangfireExtensions
    {
        public static void RegisterRecurringJobs(this IRecurringJobManager manager)
        {
            manager.AddOrUpdate<RefreshTokensCleanupJob>(
                RefreshTokensCleanupJob.JobId,
                job => job.ExecuteAsync(),
                RefreshTokensCleanupJob.Schedule
            );

        }
    }
}

