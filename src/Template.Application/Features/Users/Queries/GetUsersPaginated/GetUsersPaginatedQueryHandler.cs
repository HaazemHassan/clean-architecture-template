using MediatR;
using Template.Application.Common.Pagination;
using Template.Application.Common.Responses;
using Template.Application.Specifications.Users;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Specifications.Users;

namespace Template.Application.Features.Users.Queries.GetUsersPaginated
{
    public class GetUsersPaginatedQueryHandler : ResultHandler, IRequestHandler<GetUsersPaginatedQuery, PaginatedResult<GetUsersPaginatedQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUsersPaginatedQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetUsersPaginatedQueryResponse>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken)
        {

            var dataSpec = new UsersFilterPaginatedSpec(request.PageNumber, request.PageSize, request.Search, request.SortBy);
            var countSpec = new UsersFilterCountSpec(request.Search);

            var items = await _unitOfWork.Users.ListAsync(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Users.CountAsync(countSpec, cancellationToken);

            return PaginatedResult<GetUsersPaginatedQueryResponse>.Success(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
