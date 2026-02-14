using Ardalis.Specification;
using Template.Application.Extensions;
using Template.Application.Features.Users.Queries.GetUsersPaginated;
using Template.Domain.Entities;

namespace Template.Application.Specifications.Users {
    public class UsersFilterPaginatedSpec : Specification<DomainUser, GetUsersPaginatedQueryResponse> {
        public UsersFilterPaginatedSpec(int pageNumber, int pageSize, string? search, string? sortBy) {
            if (!string.IsNullOrEmpty(search)) {
                Query.Where(u => u.FirstName.Contains(search) || u.Email.Contains(search));
            }

            if (sortBy == "name_desc")
                Query.OrderByDescending(u => u.FirstName);
            else
                Query.OrderBy(u => u.FirstName);

            Query.Paginate(pageNumber, pageSize);

            Query.Select(u => new GetUsersPaginatedQueryResponse {
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