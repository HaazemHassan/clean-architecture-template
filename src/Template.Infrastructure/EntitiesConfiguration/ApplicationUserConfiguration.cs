using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Core.Entities.IdentityEntities;

namespace Template.Infrastructure.EntitiesConfiguration {
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser> {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder) {
            builder.ToTable("AspNetUsers", schema: "identity");

            builder.HasMany(u => u.RefreshTokens)
                   .WithOne()
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.DomainUser)
                    .WithOne()
                    .HasForeignKey<ApplicationUser>(a => a.DomainUserId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.DomainUserId).IsUnique();

        }
    }
}
