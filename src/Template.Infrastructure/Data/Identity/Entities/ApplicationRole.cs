using Microsoft.AspNetCore.Identity;

namespace Template.Infrastructure.Data.Identity.Entities
{
    internal class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
        {

        }
        public ApplicationRole(string role)
        {
            Name = role;
        }

        public virtual ICollection<RolePermission> Permissions { get; set; } = new HashSet<RolePermission>();
    }
}

