using ErrorOr;
using MediatR;
using Template.Application.Contracts.Infrastructure;
using Template.Domain.Contracts.Repositories;

namespace Template.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler(IAuthenticationService _authenticationService, IUnitOfWork _unitOfWork)
    : IRequestHandler<LogoutCommand, ErrorOr<Success>> {

    public async Task<ErrorOr<Success>> Handle(LogoutCommand request, CancellationToken cancellationToken) {
        var serviceResult = await _authenticationService.LogoutAsync(request.RefreshToken!);
        if (!serviceResult.IsError)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return serviceResult;
    }
}
