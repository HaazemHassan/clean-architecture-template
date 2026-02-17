namespace Template.Infrastructure.Data.Seeding {
    public class UserSeedDto {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
