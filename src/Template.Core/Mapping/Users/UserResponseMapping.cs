using Template.Core.Entities.UserEntities;
using Template.Core.Features.Users;

namespace Template.Core.Mapping.Users {
    public partial class UserProfile {

        public void UserResponseMapping() {
            CreateMap<DomainUser, UserResponse>()
                .ForMember(dest => dest.FullName,
                   opt => opt.MapFrom(src => src.FullName))
                .IncludeAllDerived();
        }
    }
}
