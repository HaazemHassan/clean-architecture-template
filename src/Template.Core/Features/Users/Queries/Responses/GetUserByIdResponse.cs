namespace Template.Core.Features.Users.Queries.Responses {
    public class GetUserByIdResponse : UserResponse {

        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
