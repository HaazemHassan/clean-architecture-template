using MediatR;
using Template.Core.Bases.Responses;

namespace Template.Core.Features.Users.Commands.RequestModels {
    public class ChangePasswordCommand : IRequest<Response> {
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
