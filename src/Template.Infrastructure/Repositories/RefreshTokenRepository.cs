using Microsoft.EntityFrameworkCore;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Entities;
using Template.Infrastructure.Data;

namespace Template.Infrastructure.Repositories {
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository {

        private readonly DbSet<RefreshToken> _refreshTokens;


        public RefreshTokenRepository(AppDbContext context) : base(context) {
            _refreshTokens = context.Set<RefreshToken>();
        }

    }
}
