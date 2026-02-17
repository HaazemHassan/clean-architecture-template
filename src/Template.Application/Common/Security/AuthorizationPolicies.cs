namespace Template.Application.Common.Security
{
    /// <summary>
    /// Contains constant names for authorization policies used throughout the application.
    /// Use these constants instead of hardcoded strings to ensure consistency and avoid typos.
    /// </summary>
    public static class AuthorizationPolicies
    {
        /// <summary>
        /// Policy that allows access if the user is either the owner of the resource or an Admin.
        /// Requires the command to have either a 'UserId' or 'Id' property.
        /// </summary>
        public const string SelfOrAdmin = nameof(SelfOrAdmin);

        /// <summary>
        /// Policy that allows access only if the user is the owner of the resource.
        /// Requires the command to have an 'OwnerId' property.
        /// </summary>
        public const string OnlyOwner = nameof(OnlyOwner);

        /// <summary>
        /// Policy that allows access if the user is a team member of the resource.
        /// Requires the command to have a 'TeamId' property.
        /// </summary>
        public const string TeamMember = nameof(TeamMember);

        /// <summary>
        /// Policy that allows access if the user is a manager or higher role.
        /// </summary>
        public const string ManagerOrAbove = nameof(ManagerOrAbove);
    }
}
