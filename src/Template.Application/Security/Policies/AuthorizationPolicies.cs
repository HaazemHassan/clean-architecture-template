namespace Template.Application.Security.Policies
{

    public static class AuthorizationPolicies
    {
        public const string SelfOrAdmin = nameof(SelfOrAdmin);
        public const string SelfOnly = nameof(SelfOnly);
    }
}
