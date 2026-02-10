using MediatR;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Bases.Pagination;
using Template.Core.Bases.Responses;
using Template.Core.Features.Users.Queries.Models;
using Template.Core.Features.Users.Queries.Responses;
using Template.Core.Features.Users.Queries.Specefications;

namespace Template.Core.Features.Users.Queries.Handlers {
    public class GetUsersPaginatedQueryHandler : ResponseHandler, IRequestHandler<GetUsersPaginatedQuery, PaginatedResult<GetUsersPaginatedResponse>> {
        private readonly IUnitOfWork _unitOfWork;

        public GetUsersPaginatedQueryHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetUsersPaginatedResponse>> Handle(GetUsersPaginatedQuery request, CancellationToken cancellationToken) {
            var dataSpec = new UsersFilterPaginatedSpec(request.PageNumber, request.PageSize, request.Search, request.SortBy);
            var countSpec = new UsersFilterCountSpec(request.Search);

            var items = await _unitOfWork.Users.ListAsync(dataSpec, cancellationToken);
            var totalCount = await _unitOfWork.Users.CountAsync(countSpec, cancellationToken);

            return PaginatedResult<GetUsersPaginatedResponse>.Success(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
