using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Common.Security;

namespace Template.Application.Features.Authentication.Commands.Logout
{
    [Authorize]
    public class LogoutCommand : IRequest<Result>
    {
        public string? RefreshToken { get; set; }
    }
}
