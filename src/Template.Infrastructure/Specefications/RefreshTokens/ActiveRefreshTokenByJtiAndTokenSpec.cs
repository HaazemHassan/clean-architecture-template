using Ardalis.Specification;
using Template.Core.Entities;

namespace Template.Infrastructure.Specefications.RefreshTokens {
    public class ActiveRefreshTokenByJtiAndTokenSpec : Specification<RefreshToken> {
        public ActiveRefreshTokenByJtiAndTokenSpec(string accessTokenJti, string refreshToken, int userId) {
            Query.Where(x =>
                    x.AccessTokenJTI == accessTokenJti &&
                    x.Token == refreshToken &&
                    x.UserId == userId &&
                    x.IsActive
            );
        }
    }
}
