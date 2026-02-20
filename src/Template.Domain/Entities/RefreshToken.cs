using Template.Domain.Entities.Contracts;
using Template.Domain.Primitives.Template.Domain.Primitives;

namespace Template.Domain.Entities
{
    public sealed class RefreshToken : BaseEntity<int>, IHasCreationTime
    {
        public RefreshToken(string token, DateTime expires, string accessTokenJTI, int userId)
        {
            Token = token;
            Expires = expires;
            AccessTokenJTI = accessTokenJTI;
            UserId = userId;
        }

        public int UserId { get; set; }      //ApplicationUser not DomainUser
        public string AccessTokenJTI { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? RevokationDate { get; set; }
        public bool IsActive => RevokationDate is null && !IsExpired;

        public DateTime CreatedAt { get; set; }

        public void Revoke()
        {
            RevokationDate = DateTime.UtcNow;
        }
    }
}
