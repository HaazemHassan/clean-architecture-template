namespace Template.Domain.Common;

public static class Errors
{
    public static class User
    {
        public const string EmailAlreadyExists = "EMAIL_ALREADY_EXISTS";
        public const string PhoneAlreadyExists = "PHONE_ALREADY_EXISTS";
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string EmailNotVerified = "EMAIL_NOT_VERIFIED";
    }

}
