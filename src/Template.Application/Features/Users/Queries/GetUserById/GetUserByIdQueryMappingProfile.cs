using AutoMapper;
using Template.Application.Features.Users.Common;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Queries.GetUserById {
    public class UserResponseMappingProfile : Profile {
        public UserResponseMappingProfile() {
            CreateMap<DomainUser, GetUserByIdQueryResponse>()
                           .IncludeBase<DomainUser, UserResponse>();
        }

    }
}