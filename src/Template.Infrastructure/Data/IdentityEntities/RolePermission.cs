using Template.Domain.Enums;

namespace Template.Infrastructure.Data.IdentityEntities
{
    internal class RolePermission
    {
        public int RoleId { get; set; }
        public Permission Permission { get; set; }

        public virtual ApplicationRole Role { get; set; } = null!;
    }
}
