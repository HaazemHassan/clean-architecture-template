using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Domain.Abstracts.RepositoriesAbstracts;

namespace Template.Application.Features.Authentication.Commands.Logout;

public class LogoutCommandHandler : ResponseHandler, IRequestHandler<LogoutCommand, Result> {
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork) {
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken) {
        var serviceResult = await _authenticationService.LogoutAsync(request.RefreshToken!);
        if (serviceResult.Succeeded)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(serviceResult);
    }
}
