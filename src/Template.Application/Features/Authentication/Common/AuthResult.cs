using System.Text.Json.Serialization;
using Template.Application.Features.Users.Common;

namespace Template.Application.Features.Authentication.Common
{
    public class AuthResult(string accessToken, RefreshTokenDTO refreshToken, UserResponse user)
    {
        public string AccessToken { get; set; } = accessToken;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RefreshTokenDTO? RefreshToken { get; set; } = refreshToken;

        public UserResponse User { get; set; } = user;
    }


    public class RefreshTokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
