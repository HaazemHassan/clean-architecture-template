using Template.Domain.Entities;

namespace Template.Domain.Contracts.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task DeleteExpiredTokensAsync(DateTime cutoffDate);
    }
}
