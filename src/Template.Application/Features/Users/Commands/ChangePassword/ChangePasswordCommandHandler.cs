using ErrorOr;
using MediatR;
using Template.Application.Contracts.Api;
using Template.Application.Contracts.Infrastructure;

namespace Template.Application.Features.Users.Commands.ChangePassword {

    public class ChangePasswordCommandHandler(IAuthenticationService _authenticationService, ICurrentUserService _currentUserService)
        : IRequestHandler<ChangePasswordCommand, ErrorOr<Success>> {

        public async Task<ErrorOr<Success>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken) {
            var userId = _currentUserService.UserId;
            return await _authenticationService.ChangePassword(userId!.Value, request.CurrentPassword, request.NewPassword);
        }
    }
}
