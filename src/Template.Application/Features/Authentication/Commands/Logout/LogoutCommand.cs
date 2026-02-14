using MediatR;
using Template.Application.Common.Responses;

namespace Template.Application.Features.Authentication.Commands.Logout {
    public class LogoutCommand : IRequest<Result> {
        public string? RefreshToken { get; set; }
    }
}
