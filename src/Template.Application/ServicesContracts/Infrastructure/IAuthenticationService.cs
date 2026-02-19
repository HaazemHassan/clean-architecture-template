using ErrorOr;
using Template.Application.Features.Authentication.Common;

namespace Template.Application.Contracts.Infrastructure {
    public interface IAuthenticationService {
        public Task<ErrorOr<AuthResult>> SignInWithPassword(string Email, string password, CancellationToken ct = default);
        public Task<ErrorOr<AuthResult>> ReAuthenticateAsync(string refreshToken, string accessToken, CancellationToken ct = default);
        public Task<ErrorOr<Success>> LogoutAsync(string refreshToken, CancellationToken ct = default);
        public Task<ErrorOr<Success>> ChangePassword(int domainUserId, string currentPassword, string newPassword);
    }
}
