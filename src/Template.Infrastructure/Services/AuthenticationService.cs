using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Api;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Application.Enums;
using Template.Application.Features.Authentication.Common;
using Template.Application.Features.Users.Queries.GetUserById;
using Template.Domain.Abstracts.RepositoriesAbstracts;
using Template.Domain.Entities;
using Template.Infrastructure.Common.Options;
using Template.Infrastructure.Data;
using Template.Infrastructure.Data.IdentityEntities;
using Template.Infrastructure.Specifications.RefreshTokens;

namespace Template.Infrastructure.Services {
    internal class AuthenticationService : IAuthenticationService {

        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<AuthenticationService> _logger;
        //private readonly IEmailService _emailService;



        public AuthenticationService(JwtSettings jwtSettings, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IApplicationUserService applicationUserService, IMapper mapper /*IEmailService emailService*/, RoleManager<ApplicationRole> roleManager, ICurrentUserService currentUserService, AppDbContext dbContext, ILogger<AuthenticationService> logger) {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            _logger = logger;
            //_emailService = emailService;
        }

        public async Task<ServiceOperationResult<AuthResult>> SignInWithPassword(string email, string passwod, CancellationToken ct = default) {
            var userFromDb = await _userManager.Users.Include(au => au.DomainUser)
                                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken: ct);
            if (userFromDb is null) {
                _logger.LogWarning("Failed login attempt for email: {Email} - User not found", email);
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Invalid Email or password");
            }

            bool isAuthenticated = await _userManager.CheckPasswordAsync(userFromDb, passwod);
            if (!isAuthenticated) {
                _logger.LogWarning("Failed login attempt for email: {Email} - Invalid password", email);
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Invalid Email or password");
            }

            //if (!userFromDb.EmailConfirmed)
            //return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Please confirm your email first");

            var result = await AuthenticateAsync(userFromDb);
            if (result.Succeeded) {
                _logger.LogInformation("Successful login for user: {Email}", email);
            }
            return result;
        }

        public async Task<ServiceOperationResult<AuthResult>> ReAuthenticateAsync(string refreshToken, string accessToken, CancellationToken ct = default) {
            var isValidAccessToken = ValidateAccessToken(accessToken, validateLifetime: false);
            if (!isValidAccessToken) {
                _logger.LogWarning("ReAuthenticate failed - Invalid access token");
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Invalid access token");
            }

            var jwt = ReadJWT(accessToken);
            if (jwt is null) {
                _logger.LogWarning("ReAuthenticate failed - Cannot read JWT");
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Invalid access token");
            }

            var domainUserId = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (domainUserId is null) {
                _logger.LogWarning("ReAuthenticate failed - User id is null in JWT");
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "User id is null");
            }

            var appUser = await _userManager.Users.Include(au => au.DomainUser).FirstOrDefaultAsync(au => au.DomainUserId.ToString() == domainUserId, cancellationToken: ct);
            if (appUser is null) {
                _logger.LogWarning("ReAuthenticate failed - User not found for ID: {UserId}", domainUserId);
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "User not found");
            }

            var currentRefreshTokenSpec = new ActiveRefreshTokenByJtiAndTokenSpec(jwt.Id, refreshToken, appUser.Id);
            var currentRefreshToken = await _unitOfWork.RefreshTokens.FirstOrDefaultAsync(currentRefreshTokenSpec, ct);


            if (currentRefreshToken is null || !currentRefreshToken.IsActive) {
                _logger.LogWarning("ReAuthenticate failed - Invalid or inactive refresh token for user: {Email}", appUser.Email);
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Unauthorized, "Refresh token is not valid");
            }

            //new jwt result
            var jwtResultOperation = await AuthenticateAsync(appUser, currentRefreshToken!.Expires);
            if (!jwtResultOperation.Succeeded)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.Failed, jwtResultOperation.Message);

            currentRefreshToken.Revoke();
            await _unitOfWork.RefreshTokens.UpdateAsync(currentRefreshToken, ct);
            _logger.LogInformation("Successful token refresh for user: {Email}", appUser.Email);
            return jwtResultOperation;
        }


        public async Task<ServiceOperationResult> LogoutAsync(string refreshToken, CancellationToken ct = default) {
            if (!_currentUserService.IsAuthenticated)
                return ServiceOperationResult.Failure(ServiceOperationStatus.Unauthorized, "You're already signed out!");

            int domainUserId = _currentUserService.UserId!.Value;
            var appUser = await _userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId, cancellationToken: ct);
            if (appUser is null) {
                _logger.LogWarning("Logout failed - User not found for ID: {UserId}", domainUserId);
                return ServiceOperationResult.Failure(ServiceOperationStatus.NotFound, "User not found!");
            }

            var refreshTokenFromDb = await _unitOfWork.RefreshTokens.GetAsync(r => r.Token == refreshToken && r.UserId == appUser.Id, ct);

            if (refreshTokenFromDb is null || !refreshTokenFromDb.IsActive) {
                _logger.LogWarning("Logout failed - Invalid refresh token for user: {Email}", appUser.Email);
                return ServiceOperationResult.Failure(ServiceOperationStatus.NotFound, "You maybe signed out!");
            }

            refreshTokenFromDb.Revoke();
            await _unitOfWork.RefreshTokens.UpdateAsync(refreshTokenFromDb, ct);
            _logger.LogInformation("Successful logout for user: {Email}", appUser.Email);
            return ServiceOperationResult.Success(message: "Logout successfull");

        }


        public async Task<ServiceOperationResult> ChangePassword(int domainUserId, string currentPassword, string newPassword) {
            var appUser = await _userManager.Users.FirstOrDefaultAsync(au => au.DomainUserId == domainUserId);
            if (appUser is null)
                return ServiceOperationResult.Failure(ServiceOperationStatus.NotFound, "User not found.");

            var result = await _userManager.ChangePasswordAsync(appUser, currentPassword, newPassword);

            if (!result.Succeeded)
                return ServiceOperationResult.Failure(ServiceOperationStatus.Unauthorized, result.Errors.FirstOrDefault()?.Description);

            return ServiceOperationResult.Success(ServiceOperationStatus.Updated);

        }



        #region Helper functions

        private async Task<ServiceOperationResult<AuthResult>> AuthenticateAsync(ApplicationUser appUser, DateTime? refreshTokenExpDate = null) {
            if (appUser is null || appUser.DomainUserId is null || appUser.DomainUser is null)
                return ServiceOperationResult<AuthResult>.Failure(ServiceOperationStatus.InvalidParameters, "User cannot be null");


            var userClaims = await GetUserClaims(appUser);

            var jwtSecurityToken = GenerateAccessToken(userClaims);
            var refreshToken = GenerateRefreshToken(appUser.Id, jwtSecurityToken.Id, refreshTokenExpDate);
            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);

            //prepare response
            string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            RefreshTokenDTO refreshTokenDto = new() {
                Token = refreshToken.Token,
                UserId = refreshToken.UserId,
                ExpirationDate = refreshToken.Expires
            };

            var userResponse = new GetUserByIdQueryResponse {
                Id = appUser.DomainUserId.Value,
                Email = appUser.DomainUser.Email,
                Address = appUser.DomainUser.Address!,
                PhoneNumber = appUser.DomainUser.PhoneNumber!
            };
            AuthResult jwtResult = new(accessToken, refreshTokenDto, userResponse);


            return ServiceOperationResult<AuthResult>.Success(jwtResult, message: "Logged in successfully");
        }

        private JwtSecurityToken GenerateAccessToken(List<Claim> userClaims) {
            return new JwtSecurityToken(
                  issuer: _jwtSettings.Issuer,
                  audience: _jwtSettings.Audience,
                  claims: userClaims,
                  signingCredentials: GetSigningCredentials(),
                  expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
              );
        }
        private async Task<List<Claim>> GetUserClaims(ApplicationUser user) {
            var claims = new List<Claim>()
             {
                 new(ClaimTypes.NameIdentifier,user.DomainUserId!.Value.ToString()),
                 new(ClaimTypes.Email,user.DomainUser!.Email),
                 new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

             };

            var customClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(customClaims);

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roles) {
                claims.Add(new Claim(ClaimTypes.Role, roleName));

                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null) continue;

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }

            return claims;
        }
        private SigningCredentials GetSigningCredentials() {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }
        private RefreshToken GenerateRefreshToken(int userId, string accessTokenJTI, DateTime? expirationDate = null) {
            var randomBytes = new byte[64];
            RandomNumberGenerator.Fill(randomBytes);
            string Token = Convert.ToBase64String(randomBytes);

            expirationDate = expirationDate ?? DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
            return new RefreshToken(Token, expirationDate.Value, accessTokenJTI, userId);

        }


        private bool ValidateAccessToken(string token, bool validateLifetime = true) {
            var tokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.FromMinutes(2)  //default = 5 min (security gap)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return principal is not null;
        }

        private static JwtSecurityToken ReadJWT(string accessToken) {
            if (string.IsNullOrEmpty(accessToken)) {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }

        #endregion

    }
}