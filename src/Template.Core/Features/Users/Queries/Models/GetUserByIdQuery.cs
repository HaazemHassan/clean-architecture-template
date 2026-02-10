using MediatR;
using Template.Core.Bases.Responses;
using Template.Core.Features.Users.Queries.Responses;

namespace Template.Core.Features.Users.Queries.Models {
    public class GetUserByIdQuery : IRequest<Response<GetUserByIdResponse>> {
        public int Id { get; set; }
    }
}
