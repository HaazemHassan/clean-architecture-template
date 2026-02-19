using AutoMapper;
using ErrorOr;
using MediatR;
using Template.Application.Specifications.Users;
using Template.Domain.Common.Constants;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<GetUserByIdQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<GetUserByIdQueryResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetByIdSpec<DomainUser>(request.OwnerUserId);
            var user = await _unitOfWork.Users.GetAsync<GetUserByIdQueryResponse>(spec, cancellationToken);
            if (user is null)
                return Error.NotFound(code: ErrorCodes.User.UserNotFound, description: "User not found");

            return user;
        }
    }
}
