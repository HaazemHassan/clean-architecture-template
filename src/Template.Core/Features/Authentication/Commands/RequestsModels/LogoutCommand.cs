using MediatR;
using Template.Core.Bases.Responses;

namespace Template.Core.Features.Authentication.Commands.RequestsModels {
    public class LogoutCommand : IRequest<Response> {
        public string? RefreshToken { get; set; }
    }
}
