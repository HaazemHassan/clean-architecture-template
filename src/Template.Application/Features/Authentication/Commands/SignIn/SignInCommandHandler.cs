using ErrorOr;
using MediatR;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Features.Authentication.Common;
using Template.Domain.Contracts.Repositories;

namespace Template.Application.Features.Authentication.Commands.SignIn;

public class SignInCommandHandler(IAuthenticationService _authenticationService, IUnitOfWork _unitOfWork)
    : IRequestHandler<SignInCommand, ErrorOr<AuthResult>> {

    public async Task<ErrorOr<AuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken) {
        var authResult = await _authenticationService.SignInWithPassword(request.Email, request.Password);
        if (!authResult.IsError)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return authResult;
    }
}
