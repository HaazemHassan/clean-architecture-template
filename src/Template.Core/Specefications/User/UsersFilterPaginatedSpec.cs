using Ardalis.Specification;
using Template.Core.Entities.UserEntities;
using Template.Core.Extensions;
using Template.Core.Features.Users.Queries.Responses;

namespace Template.Core.Specefications.User {
    public class UsersFilterPaginatedSpec : Specification<DomainUser, GetUsersPaginatedResponse> {
        public UsersFilterPaginatedSpec(int pageNumber, int pageSize, string? search, string? sortBy) {
            if (!string.IsNullOrEmpty(search)) {
                Query.Where(u => u.FirstName.Contains(search) || u.Email.Contains(search));
            }

            if (sortBy == "name_desc")
                Query.OrderByDescending(u => u.FirstName);
            else
                Query.OrderBy(u => u.FirstName);

            Query.Paginate(pageNumber, pageSize);

            Query.Select(u => new GetUsersPaginatedResponse {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Address = u.Address ?? string.Empty,
                Phone = u.PhoneNumber
            });
            Query.AsNoTracking();

        }
    }
}