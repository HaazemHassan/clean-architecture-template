using Template.Core.Entities.UserEntities;
using Template.Core.Features.Users;
using Template.Core.Features.Users.Queries.Responses;

namespace Template.Core.Mapping.Users {
    public partial class UserProfile {
        public void GetUsersPaginatedMapping() {
            CreateMap<DomainUser, GetUsersPaginatedResponse>()
                .IncludeBase<DomainUser, UserResponse>()
                .ForMember(dest => dest.Phone,
                   opt => opt.MapFrom(src => src.PhoneNumber));

        }
    }
}


