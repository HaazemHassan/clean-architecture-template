using Template.Application.Features.Users.Common;

namespace Template.Application.Features.Users.Queries.GetUserById {
    public class GetUserByIdQueryResponse : UserResponse {

        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
