using ErrorOr;
using MediatR;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Features.Authentication.Common;
using Template.Domain.Contracts.Repositories;

namespace Template.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(IAuthenticationService _authenticationService, IUnitOfWork _unitOfWork)
    : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthResult>> {

    public async Task<ErrorOr<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) {
        var authResult = await _authenticationService.ReAuthenticateAsync(request.RefreshToken!, request.AccessToken);
        if (!authResult.IsError)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return authResult;
    }
}
