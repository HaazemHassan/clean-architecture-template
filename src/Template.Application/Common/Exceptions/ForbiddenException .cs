namespace Template.Application.Common.Exceptions
{
    public sealed class ForbiddenException : Exception
    {

        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException() : base("You do not have permission to perform this action.")
        {
        }
    }

}
