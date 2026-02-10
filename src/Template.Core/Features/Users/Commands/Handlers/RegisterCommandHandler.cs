using AutoMapper;
using MediatR;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Abstracts.InfrastructureAbstracts.Services;
using Template.Core.Bases.Authentication;
using Template.Core.Bases.Responses;
using Template.Core.Entities.UserEntities;
using Template.Core.Features.Users.Commands.RequestModels;

namespace Template.Core.Features.Users.Commands.Handlers {
    public class RegisterCommandHandler : ResponseHandler, IRequestHandler<RegisterCommand, Response<AuthResult>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IAuthenticationService _authenticationService;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService, IAuthenticationService authenticationService) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _authenticationService = authenticationService;
        }

        public async Task<Response<AuthResult>> Handle(RegisterCommand request, CancellationToken cancellationToken) {
            var userMapped = _mapper.Map<DomainUser>(request);
            var addUserResult = await _applicationUserService.AddUser(userMapped, request.Password, ct: cancellationToken);

            if (!addUserResult.Succeeded)
                return FromServiceResult<AuthResult>(addUserResult);

            var user = addUserResult.Data;

            var authResult = await _authenticationService.SignInWithPassword(user.Email, request.Password, cancellationToken);
            if (!authResult.Succeeded)
                return FromServiceResult(authResult);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return FromServiceResult(authResult);
        }
    }
}
