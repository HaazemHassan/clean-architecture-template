using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Core.Entities.UserEntities;

namespace Template.Infrastructure.EntitiesConfiguration {
    public class DomainUserConfiguration : IEntityTypeConfiguration<DomainUser> {
        public void Configure(EntityTypeBuilder<DomainUser> builder) {
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.PhoneNumber).IsUnique();
        }
    }
}
