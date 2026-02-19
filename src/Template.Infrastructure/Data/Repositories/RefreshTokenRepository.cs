using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Entities;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.Data.Repositories
{
    internal class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {

        private readonly DbSet<RefreshToken> _refreshTokens;


        public RefreshTokenRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _refreshTokens = context.Set<RefreshToken>();
        }

        public async Task DeleteExpiredTokensAsync(DateTime cutoffDate)
        {

            await _refreshTokens
                .Where(x => (x.RevokationDate != null && x.RevokationDate <= cutoffDate) ||
                            (x.Expires <= cutoffDate))
                .ExecuteDeleteAsync();
        }
    }
}
