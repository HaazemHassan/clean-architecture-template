namespace Template.Infrastructure.Data.Seeding {
    public class RoleSeedDto {
        public string Name { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
    }
}
