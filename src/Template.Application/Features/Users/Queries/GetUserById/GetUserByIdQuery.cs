using MediatR;
using Template.Application.Common.Responses;

namespace Template.Application.Features.Users.Queries.GetUserById {
    public class GetUserByIdQuery : IRequest<Response<GetUserByIdQueryResponse>> {
        public int Id { get; set; }
    }
}
