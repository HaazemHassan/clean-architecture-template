using MediatR;
using Template.Core.Bases.Pagination;
using Template.Core.Features.Users.Queries.Responses;

namespace Template.Core.Features.Users.Queries.Models {
    public class GetUsersPaginatedQuery : PaginatedQuery, IRequest<PaginatedResult<GetUsersPaginatedResponse>> {

    }
}
