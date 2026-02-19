using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Infrastructure.Data.Identity.Entities;

namespace Template.Infrastructure.Data.EntitiesConfigurations
{
    internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions");

            builder.HasKey(rp => new { rp.RoleId, rp.Permission });

            builder.Property(rp => rp.Permission)
                   .HasConversion<string>();
        }
    }
}
