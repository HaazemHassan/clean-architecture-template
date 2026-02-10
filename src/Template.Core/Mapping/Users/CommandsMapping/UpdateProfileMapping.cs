using Template.Core.Entities.UserEntities;
using Template.Core.Features.Users;
using Template.Core.Features.Users.Commands.Responses;

namespace Template.Core.Mapping.Users {
    public partial class UserProfile {
        public void UpdateUserProfileMapping() {
            CreateMap<DomainUser, UpdateProfileResponse>()
                .IncludeBase<DomainUser, UserResponse>();
        }
    }
}
