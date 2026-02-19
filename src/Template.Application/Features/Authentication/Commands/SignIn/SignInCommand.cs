using ErrorOr;
using MediatR;
using Template.Application.Features.Authentication.Common;

namespace Template.Application.Features.Authentication.Commands.SignIn;

public class SignInCommand : IRequest<ErrorOr<AuthResult>> {
    public string Email { get; set; }
    public string Password { get; set; }

}
