using ErrorOr;
using MediatR;

namespace Template.Application.Features.Users.Commands.ChangePassword
{

    public class ChangePasswordCommand : IRequest<ErrorOr<Success>>
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
