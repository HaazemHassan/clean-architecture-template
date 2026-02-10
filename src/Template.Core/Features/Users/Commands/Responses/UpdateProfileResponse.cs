namespace Template.Core.Features.Users.Commands.Responses {
    public class UpdateProfileResponse : UserResponse {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
