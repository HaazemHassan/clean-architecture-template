using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Features.Authentication.Common;

namespace Template.Application.Features.Authentication.Commands.SignIn;

public class SignInCommand : IRequest<Result<AuthResult>> {
    public string Email { get; set; }
    public string Password { get; set; }

}
