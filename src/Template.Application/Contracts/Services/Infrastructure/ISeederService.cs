using Template.Infrastructure.Data.Seeding;

namespace Template.Application.Contracts.Services.Infrastructure {
    public interface ISeederService {
        Task SeedRolesAsync(List<string> data, CancellationToken cancellationToken = default);
        Task SeedUsersAsync(List<UserSeedDto> data, CancellationToken cancellationToken = default);

    }
}
