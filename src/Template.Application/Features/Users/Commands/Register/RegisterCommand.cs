using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Contracts;
using Template.Application.Features.Users.Common;

namespace Template.Application.Features.Users.Commands.Register
{
    public class RegisterCommand : IRequest<Response<UserResponse>>, ITransactionalRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
