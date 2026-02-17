using AutoMapper;
using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Specifications.Users;
using Template.Domain.Contracts.Repositories;

namespace Template.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : ResultHandler, IRequestHandler<GetUserByIdQuery, Result<GetUserByIdQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetUserByIdQueryResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new UserDetailsProjectionSpec(request.Id);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(spec);
            if (user is null)
                return NotFound<GetUserByIdQueryResponse>();

            var userResponse = _mapper.Map<GetUserByIdQueryResponse>(user);
            return Success(userResponse);
        }
    }
}
