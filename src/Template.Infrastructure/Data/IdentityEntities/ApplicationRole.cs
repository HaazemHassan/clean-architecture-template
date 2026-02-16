using Microsoft.AspNetCore.Identity;

namespace Template.Infrastructure.Data.IdentityEntities
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

