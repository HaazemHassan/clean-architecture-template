using Template.Application.Common.Responses;
using Template.Domain.Entities;
using Template.Domain.Enums;

namespace Template.Application.Contracts.Services.Infrastructure {
    public interface IApplicationUserService {
        public Task<ServiceOperationResult<DomainUser>> AddUser(DomainUser user, string password, UserRole role = UserRole.User, CancellationToken ct = default);


    }

}