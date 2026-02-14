using Ardalis.Specification;
using Template.Application.Features.Users.Queries.GetUserById;
using Template.Domain.Entities;

namespace Template.Application.Specifications.Users {
    public class UserDetailsProjectionSpec : Specification<DomainUser, GetUserByIdQueryResponse> {
        public UserDetailsProjectionSpec(int userId) {
            Query.Where(u => u.Id == userId);

            Query.Select(u => new GetUserByIdQueryResponse {
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