using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Application.Features.Authentication.Common;
using Template.Domain.Contracts.Repositories;

namespace Template.Application.Features.Authentication.Commands.SignIn;

public class SignInCommandHandler : ResultHandler, IRequestHandler<SignInCommand, Result<AuthResult>> {
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;

    public SignInCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork) {
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken) {
        var authResult = await _authenticationService.SignInWithPassword(request.Email, request.Password);
        if (authResult.Succeeded)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(authResult);
    }
}
