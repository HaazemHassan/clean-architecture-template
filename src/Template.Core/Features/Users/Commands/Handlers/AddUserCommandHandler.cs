using AutoMapper;
using MediatR;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Bases.Responses;
using Template.Core.Entities.UserEntities;
using Template.Core.Features.Users.Commands.RequestModels;
using Template.Core.Features.Users.Commands.Responses;

namespace Template.Core.Features.Users.Commands.Handlers {
    public class AddUserCommandHandler : ResponseHandler, IRequestHandler<AddUserCommand, Response<AddUserResponse>> {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;

        public AddUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
        }

        public async Task<Response<AddUserResponse>> Handle(AddUserCommand request, CancellationToken cancellationToken) {
            var domainUser = _mapper.Map<DomainUser>(request);
            var addUserResult = await _applicationUserService.AddUser(domainUser, request.Password, request.UserRole, cancellationToken);

            if (!addUserResult.Succeeded)
                return FromServiceResult<AddUserResponse>(addUserResult);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<AddUserResponse>(addUserResult.Data);
            return Created(response);
        }
    }
}
