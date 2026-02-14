using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Infrastructure.Data.IdentityEntities;

namespace Template.Infrastructure.Data.EntitiesConfiguration {
    internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser> {
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
            builder.HasIndex(u => u.PhoneNumber).IsUnique();


        }
    }
}
