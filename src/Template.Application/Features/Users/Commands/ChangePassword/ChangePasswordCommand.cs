using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Common.Security;

namespace Template.Application.Features.Users.Commands.ChangePassword
{
    // User can only change their own password (authenticated users only)
    // No additional authorization needed as CurrentUserService ensures they can only change their own
    public class ChangePasswordCommand : IRequest<Result>
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
