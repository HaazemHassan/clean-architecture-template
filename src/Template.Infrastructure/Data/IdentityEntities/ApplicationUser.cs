using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Template.Domain.Entities;

namespace Template.Infrastructure.Data.IdentityEntities
{
    internal class ApplicationUser : IdentityUser<int>
    {


        public ApplicationUser(string email, string phoneNumber)
        {
            UserName = email;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public int? DomainUserId { get; private set; }

        [ForeignKey(nameof(DomainUserId))]
        public virtual DomainUser? DomainUser { get; private set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; private set; } = new HashSet<RefreshToken>();


        public void AssignDomainUser(DomainUser domainUser)
        {
            if (domainUser is null)
                throw new ArgumentNullException("Domain user can't be null");

            if (domainUser.Id != 0 && DomainUserId != domainUser.Id)
                throw new InvalidOperationException("Domain user ID mismatch");

            DomainUser = domainUser;
        }

        public void ConfirmEmail()
        {
            if (EmailConfirmed)
                throw new InvalidOperationException("Email is already confirmed.");
            EmailConfirmed = true;
        }
    }
}
