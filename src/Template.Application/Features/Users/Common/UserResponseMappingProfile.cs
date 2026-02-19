using AutoMapper;
using Template.Application.Features.Users.Common;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Mapping;

public partial class UserResponseMappingProfile : Profile
{
    public UserResponseMappingProfile()
    {
        CreateMap<DomainUser, UserResponse>()
            .ForMember(dest => dest.FullName,
               opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                  .IncludeAllDerived();
    }
}
