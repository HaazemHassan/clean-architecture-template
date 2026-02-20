namespace Template.Application.Common.Options
{

    public class PasswordOptions
    {
        public const string SectionName = "PasswordSettings";


        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public int RequiredUniqueChars { get; set; }
    }
}
