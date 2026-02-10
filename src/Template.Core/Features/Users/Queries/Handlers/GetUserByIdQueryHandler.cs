using AutoMapper;
using MediatR;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Bases.Responses;
using Template.Core.Features.Users.Queries.Models;
using Template.Core.Features.Users.Queries.Responses;
using Template.Core.Specefications.User;

namespace Template.Core.Features.Users.Queries.Handlers {
    public class GetUserByIdQueryHandler : ResponseHandler, IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>> {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork) {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) {
            var spec = new UserDetailsProjectionSpec(request.Id);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(spec);
            if (user is null)
                return NotFound<GetUserByIdResponse>();

            var userResponse = _mapper.Map<GetUserByIdResponse>(user);
            return Success(userResponse);
        }
    }
}
