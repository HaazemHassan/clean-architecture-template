using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Features.Authentication.Common;

namespace Template.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<Result<AuthResult>>
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }

}
