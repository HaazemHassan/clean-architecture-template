using Ardalis.Specification;
using Template.Domain.Entities;

namespace Template.Domain.Specifications.Users
{
    public class UsersSearchSpec : Specification<DomainUser>
    {
        public UsersSearchSpec(string? search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search) || u.Email.Contains(search));
            }
        }
    }
}
