using Microsoft.AspNetCore.Identity;

namespace Template.Infrastructure.Data.Identity
{

    public static class IdentityResultExtensions
    {
        public static bool HasError(this IdentityResult result, string errorCode)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            if (result.Succeeded)
                return false;

            if (errorCode == null || errorCode.Length == 0)
                throw new ArgumentException("At least one error code must be provided.", nameof(errorCode));

            return result.Errors.Any(e => string.Equals(e.Code, errorCode));

        }
    }
}
