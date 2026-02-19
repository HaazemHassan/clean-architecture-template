using Ardalis.Specification;
using Template.Domain.Primitives.Template.Domain.Primitives;

namespace Template.Application.Specifications.Users
{
    public class GetByIdSpec<T> : Specification<T> where T : BaseEntity<int>
    {
        public GetByIdSpec(int userId)
        {
            Query.Where(u => u.Id == userId);

        }
    }
}