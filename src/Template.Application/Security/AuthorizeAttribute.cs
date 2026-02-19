using Template.Domain.Common.Enums;

namespace Template.Application.Security
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute
    {
        public Permission[] Permissions { get; set; } = [];
        public UserRole[] Roles { get; set; } = [];
        public string? Policy { get; set; }
    }
}
