using Template.Core.Entities.UserEntities;
using Template.Core.Features.Users;
using Template.Core.Features.Users.Commands.RequestModels;
using Template.Core.Features.Users.Commands.Responses;

namespace Template.Core.Mapping.Users {
    public partial class UserProfile {
        public void AddUserMapping() {
            CreateMap<AddUserCommand, DomainUser>();
            CreateMap<DomainUser, AddUserResponse>()
                .IncludeBase<DomainUser, UserResponse>();
        }
    }
}
