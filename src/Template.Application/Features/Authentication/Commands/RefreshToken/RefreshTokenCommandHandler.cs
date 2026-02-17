using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Application.Features.Authentication.Common;
using Template.Domain.Contracts.Repositories;

namespace Template.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : ResultHandler, IRequestHandler<RefreshTokenCommand, Result<AuthResult>> {
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork) {
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) {
        var authResult = await _authenticationService.ReAuthenticateAsync(request.RefreshToken!, request.AccessToken);
        if (authResult.Succeeded)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(authResult);
    }
}
