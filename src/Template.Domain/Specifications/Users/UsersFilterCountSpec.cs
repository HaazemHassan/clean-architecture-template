using Ardalis.Specification;
using Template.Domain.Entities;

namespace Template.Domain.Specifications.Users {
    public class UsersFilterCountSpec : Specification<DomainUser> {
        public UsersFilterCountSpec(string? search) {
            if (!string.IsNullOrEmpty(search)) {
                Query.Where(u => u.FirstName.Contains(search) || u.Email.Contains(search));
            }
        }
    }
}
