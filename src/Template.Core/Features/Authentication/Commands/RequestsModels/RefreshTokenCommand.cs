using MediatR;
using Template.Core.Bases.Authentication;
using Template.Core.Bases.Responses;

namespace Template.Core.Features.Authentication.Commands.RequestsModels;

public class RefreshTokenCommand : IRequest<Response<AuthResult>> {
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }

}
