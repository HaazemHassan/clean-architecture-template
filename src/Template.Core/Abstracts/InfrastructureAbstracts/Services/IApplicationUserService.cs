using Template.Core.Bases.Responses;
using Template.Core.Entities.UserEntities;
using Template.Core.Enums;

namespace Template.Core.Abstracts.InfrastructureAbstracts.Repositories {
    public interface IApplicationUserService {
        public Task<ServiceOperationResult<DomainUser>> AddUser(DomainUser user, string password, UserRole role = UserRole.User, CancellationToken ct = default);


    }

}