using MediatR;
using Template.Core.Bases.Authentication;
using Template.Core.Bases.Responses;

namespace Template.Core.Features.Authentication.Commands.RequestsModels;

public class SignInCommand : IRequest<Response<AuthResult>> {
    public string Email { get; set; }
    public string Password { get; set; }

}
