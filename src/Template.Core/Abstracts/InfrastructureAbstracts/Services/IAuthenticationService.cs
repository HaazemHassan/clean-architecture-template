using Template.Core.Bases.Authentication;
using Template.Core.Bases.Responses;

namespace Template.Core.Abstracts.InfrastructureAbstracts.Services {
    public interface IAuthenticationService {
        public Task<ServiceOperationResult<AuthResult>> SignInWithPassword(string Email, string passwod, CancellationToken ct = default);
        public Task<ServiceOperationResult<AuthResult>> ReAuthenticateAsync(string refreshToken, string accessToken, CancellationToken ct = default);
        public Task<ServiceOperationResult> LogoutAsync(string refreshToken, CancellationToken ct = default);
        public Task<ServiceOperationResult> ChangePassword(int domainUserId, string currentPassword, string newPassword);


    }
}
