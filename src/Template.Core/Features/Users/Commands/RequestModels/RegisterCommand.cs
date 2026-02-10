using MediatR;
using Template.Core.Bases.Authentication;
using Template.Core.Bases.Responses;

namespace Template.Core.Features.Users.Commands.RequestModels {
    public class RegisterCommand : IRequest<Response<AuthResult>> {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
