using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Common.Security;
using Template.Application.Contracts.Requests;
using Template.Domain.Enums;

namespace Template.Application.Features.Users.Commands.AddUser
{
    [Authorize(Permissions = [Permission.UsersWrite])]
    public class AddUserCommand : IRequest<Result<AddUserCommandResponse>>, ITransactionalRequest, IAuthorizedRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public UserRole UserRole { get; set; } = UserRole.User;

    }
}
