using Template.Application.Features.Users.Common;

namespace Template.Application.Features.Users.Queries.GetUsersPaginated {
    public class GetUsersPaginatedQueryResponse : UserResponse {

        public string? Address { get; set; }
        public string Phone { get; set; } = string.Empty;

    }
}
