using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Template.Core.Entities.UserEntities;

namespace Template.Core.Entities.IdentityEntities {
    public class ApplicationUser : IdentityUser<int> {

        public int? DomainUserId { get; set; }

        [ForeignKey(nameof(DomainUserId))]
        public virtual DomainUser? DomainUser { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();


        public static ApplicationUser Create(string email, string phoneNumber) {
            return new ApplicationUser {
                UserName = email,
                Email = email,
                PhoneNumber = phoneNumber
            };
        }

        public void AssignDomainUser(DomainUser domainUser) {
            if (domainUser is null)
                throw new ArgumentNullException("Domain user can't be null");

            if (domainUser.Id != 0 && DomainUserId != domainUser.Id)
                throw new InvalidOperationException("Domain user ID mismatch");

            DomainUser = domainUser;
        }

        public void ConfirmEmail() {
            EmailConfirmed = true;
        }
    }
}
