using MediatR;
using Template.Application.Common.Pagination;

namespace Template.Application.Features.Users.Queries.GetUsersPaginated {
    public class GetUsersPaginatedQuery : PaginatedQuery, IRequest<PaginatedResult<GetUsersPaginatedQueryResponse>> {

    }
}
