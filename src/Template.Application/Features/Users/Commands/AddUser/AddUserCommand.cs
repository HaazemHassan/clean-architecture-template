using MediatR;
using Template.Application.Common.Responses;
using Template.Domain.Enums;

namespace Template.Application.Features.Users.Commands.AddUser {
    public class AddUserCommand : IRequest<Response<AddUserCommandResponse>> {
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
