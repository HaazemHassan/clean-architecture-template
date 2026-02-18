using AutoMapper;
using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Application.Features.Users.Common;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Commands.Register
{
    public class RegisterCommandHandler : ResultHandler, IRequestHandler<RegisterCommand, Result<UserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IAuthenticationService _authenticationService;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _authenticationService = authenticationService;
        }

        public async Task<Result<UserResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userToAdd = new DomainUser(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Address);

            var addUserResult = await _applicationUserService.AddUser(userToAdd, request.Password, ct: cancellationToken);
            
            if (!addUserResult.Succeeded)
                return FromServiceResult<UserResponse>(addUserResult);

            var userResponse = _mapper.Map<UserResponse>(addUserResult.Data);
            return Created(userResponse, addUserResult.Message);
        }
    }
}
