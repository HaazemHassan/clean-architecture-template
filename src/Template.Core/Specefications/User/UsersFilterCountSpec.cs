using Ardalis.Specification;
using Template.Core.Entities.UserEntities;

namespace Template.Core.Specefications.User {
    public class UsersFilterCountSpec : Specification<DomainUser> {
        public UsersFilterCountSpec(string? search) {
            if (!string.IsNullOrEmpty(search)) {
                Query.Where(u => u.FirstName.Contains(search) || u.Email.Contains(search));
            }
        }
    }
}
