using Template.Core.Entities.Abstracts;

namespace Template.Core.Entities {
    public class RefreshToken : BaseEntity<int>, IHasCreationTime {
        public int Id { get; set; }
        public int UserId { get; set; }      //ApplicationUser not DomainUser
        public string? AccessTokenJTI { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? RevokationDate { get; set; }
        public bool IsActive => RevokationDate is null && !IsExpired;

        public DateTime CreatedAt { get; set; }

        public void Revoke() {
            RevokationDate = DateTime.UtcNow;
        }
    }
}
