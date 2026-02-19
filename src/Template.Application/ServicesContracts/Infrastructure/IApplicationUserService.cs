using ErrorOr;
using Template.Domain.Common.Enums;
using Template.Domain.Entities;

namespace Template.Application.Contracts.Infrastructure
{
    public interface IApplicationUserService
    {
        public Task<ErrorOr<DomainUser>> AddUser(DomainUser user, string password, UserRole role = UserRole.User, CancellationToken ct = default);


    }

}