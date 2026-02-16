using AutoMapper;
using Template.Application.Features.Users.Common;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryMappingProfile : Profile
    {
        public GetUserByIdQueryMappingProfile()
        {
            CreateMap<DomainUser, GetUserByIdQueryResponse>()
                           .IncludeBase<DomainUser, UserResponse>();
        }

    }
}