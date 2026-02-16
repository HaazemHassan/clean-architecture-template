using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Api;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Domain.Contracts.Repositories;

namespace Template.Application.Features.Users.Commands.ChangePassword {

    public class ChangePasswordCommandHandler : ResultHandler, IRequestHandler<ChangePasswordCommand, Result> {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICurrentUserService _currentUserService;


        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IAuthenticationService authenticationService, ICurrentUserService currentUserService) {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken) {
            var userId = _currentUserService.UserId;
            var changeResult = await _authenticationService.ChangePassword(userId!.Value, request.CurrentPassword, request.NewPassword);
            return FromServiceResult(changeResult);

        }
    }
}
