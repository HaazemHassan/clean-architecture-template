using ErrorOr;
using MediatR;
using Template.Application.Security;

namespace Template.Application.Features.Authentication.Commands.Logout
{
    [Authorize]
    public class LogoutCommand : IRequest<ErrorOr<Success>>
    {
        public string? RefreshToken { get; set; }
    }
}
