using Template.Application.Features.Users.Common;

namespace Template.Application.Features.Users.Commands.UpdateProfile {
    public class UpdateProfileCommandResponse : UserResponse {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
