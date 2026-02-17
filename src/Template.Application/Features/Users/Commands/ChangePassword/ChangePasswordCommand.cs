using MediatR;
using Template.Application.Common.Responses;

namespace Template.Application.Features.Users.Commands.ChangePassword
{

    public class ChangePasswordCommand : IRequest<Result>
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
