using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Domain.Entities;

namespace Template.Infrastructure.Data.Configurations {
    public class DomainUserConfigurations : IEntityTypeConfiguration<DomainUser> {
        public void Configure(EntityTypeBuilder<DomainUser> builder) {
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.PhoneNumber).IsUnique();
        }
    }
}
