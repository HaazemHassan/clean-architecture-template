using MediatR;
using Template.Core.Abstracts.ApiAbstracts;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Abstracts.InfrastructureAbstracts.Services;
using Template.Core.Bases.Responses;
using Template.Core.Features.Users.Commands.RequestModels;

namespace Template.Core.Features.Users.Commands.Handlers {

    public class ChangePasswordCommandHandler : ResponseHandler, IRequestHandler<ChangePasswordCommand, Response> {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICurrentUserService _currentUserService;


        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IAuthenticationService authenticationService, ICurrentUserService currentUserService) {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _currentUserService = currentUserService;
        }

        public async Task<Response> Handle(ChangePasswordCommand request, CancellationToken cancellationToken) {
            var userId = _currentUserService.UserId;
            var changeResult = await _authenticationService.ChangePassword(userId!.Value, request.CurrentPassword, request.NewPassword);
            return FromServiceResult(changeResult);

        }
    }
}
