using Ardalis.Specification;
using Template.Core.Entities.UserEntities;
using Template.Core.Features.Users.Queries.Responses;

namespace Template.Core.Features.Users.Queries.Specefications {
    public class UserDetailsProjectionSpec : Specification<DomainUser, GetUserByIdResponse> {
        public UserDetailsProjectionSpec(int userId) {
            Query.Where(u => u.Id == userId);

            Query.Select(u => new GetUserByIdResponse {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Address = u.Address ?? string.Empty,
                PhoneNumber = u.PhoneNumber
            });
        }
    }
}