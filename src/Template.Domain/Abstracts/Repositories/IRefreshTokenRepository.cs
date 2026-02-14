using Template.Domain.Entities;

namespace Template.Domain.Abstracts.RepositoriesAbstracts {
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken> {
        Task DeleteExpiredTokensAsync(DateTime cutoffDate);
    }
}
