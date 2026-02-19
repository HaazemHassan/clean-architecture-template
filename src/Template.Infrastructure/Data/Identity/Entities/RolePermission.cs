using Template.Domain.Common.Enums;

namespace Template.Infrastructure.Data.Identity.Entities
{
    internal class RolePermission
    {
        public int RoleId { get; set; }
        public Permission Permission { get; set; }

        public virtual ApplicationRole Role { get; set; } = null!;
    }
}
