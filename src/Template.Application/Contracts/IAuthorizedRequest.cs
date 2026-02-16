using Template.Domain.Enums;

namespace Template.Application.Contracts
{
    public interface IAuthorizedRequest
    {
        IEnumerable<Permission> RequiredPermissions { get; }
    }

}
