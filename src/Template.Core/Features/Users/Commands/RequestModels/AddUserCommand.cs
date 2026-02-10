using MediatR;
using Template.Core.Bases.Responses;
using Template.Core.Enums;
using Template.Core.Features.Users.Commands.Responses;

namespace Template.Core.Features.Users.Commands.RequestModels {
    public class AddUserCommand : IRequest<Response<AddUserResponse>> {
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
