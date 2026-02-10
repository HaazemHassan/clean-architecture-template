using MediatR;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Abstracts.InfrastructureAbstracts.Services;
using Template.Core.Bases.Authentication;
using Template.Core.Bases.Responses;
using Template.Core.Features.Authentication.Commands.RequestsModels;

namespace Template.Core.Features.Authentication.Commands.Handlers;

public class SignInCommandHandler : ResponseHandler, IRequestHandler<SignInCommand, Response<AuthResult>> {
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;

    public SignInCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork) {
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<AuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken) {
        var authResult = await _authenticationService.SignInWithPassword(request.Email, request.Password);
        if (authResult.Succeeded)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(authResult);
    }
}
