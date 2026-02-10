using System.Text.Json.Serialization;
using Template.Core.Features.Users.Queries.Responses;

namespace Template.Core.Bases.Authentication {
    public class AuthResult(string accessToken, RefreshTokenDTO refreshToken, GetUserByIdResponse user) {
        public string AccessToken { get; set; } = accessToken;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RefreshTokenDTO? RefreshToken { get; set; } = refreshToken;

        public GetUserByIdResponse User { get; set; } = user;
    }


    public class RefreshTokenDTO {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
