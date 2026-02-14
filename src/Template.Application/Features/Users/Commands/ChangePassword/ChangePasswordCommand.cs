using MediatR;
using Template.Application.Common.Responses;

namespace Template.Application.Features.Users.Commands.ChangePassword {
    public class ChangePasswordCommand : IRequest<Result> {
        public ChangePasswordCommand() {
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmNewPassword = string.Empty;
        }

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
