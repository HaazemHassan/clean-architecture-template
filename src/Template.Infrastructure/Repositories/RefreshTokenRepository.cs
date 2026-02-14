using Microsoft.EntityFrameworkCore;
using Template.Domain.Abstracts.RepositoriesAbstracts;
using Template.Domain.Entities;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.Repositories {
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository {

        private readonly DbSet<RefreshToken> _refreshTokens;


        public RefreshTokenRepository(AppDbContext context) : base(context) {
            _refreshTokens = context.Set<RefreshToken>();
        }

        public async Task DeleteExpiredTokensAsync(DateTime cutoffDate) {

            await _refreshTokens
                .Where(x => (x.RevokationDate != null && x.RevokationDate <= cutoffDate) ||
                            (x.Expires <= cutoffDate))
                .ExecuteDeleteAsync();
        }
    }
}
