namespace Template.Infrastructure.Data.Seeding
{
    public interface ISeederService
    {
        Task SeedRolesAsync(List<RoleSeedDto> data, CancellationToken cancellationToken = default);
        Task SeedUsersAsync(List<UserSeedDto> data, CancellationToken cancellationToken = default);

    }
}

