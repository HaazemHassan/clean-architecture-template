using AutoMapper;
using Template.Application.Features.Users.Common;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Queries.GetUsersPaginated {
    public class UserResponseMappingProfile : Profile {
        public UserResponseMappingProfile() {
            CreateMap<DomainUser, GetUsersPaginatedQueryResponse>()
           .IncludeBase<DomainUser, UserResponse>()
           .ForMember(dest => dest.Phone,
              opt => opt.MapFrom(src => src.PhoneNumber));
        }

    }
}


