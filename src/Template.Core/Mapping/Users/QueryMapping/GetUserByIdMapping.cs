using Template.Core.Entities.UserEntities;
using Template.Core.Features.Users;
using Template.Core.Features.Users.Queries.Responses;

namespace Template.Core.Mapping.Users {
    public partial class UserProfile {
        public void GetUserByIdMapping() {
            CreateMap<DomainUser, GetUserByIdResponse>()
                .IncludeBase<DomainUser, UserResponse>();
        }
    }
}