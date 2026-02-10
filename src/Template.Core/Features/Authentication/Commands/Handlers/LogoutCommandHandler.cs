using MediatR;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Abstracts.InfrastructureAbstracts.Services;
using Template.Core.Bases.Responses;
using Template.Core.Features.Authentication.Commands.RequestsModels;

namespace Template.Core.Features.Authentication.Commands.Handlers;

public class LogoutCommandHandler : ResponseHandler, IRequestHandler<LogoutCommand, Response> {
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork) {
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response> Handle(LogoutCommand request, CancellationToken cancellationToken) {
        var serviceResult = await _authenticationService.LogoutAsync(request.RefreshToken!);
        if (serviceResult.Succeeded)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return FromServiceResult(serviceResult);
    }
}
