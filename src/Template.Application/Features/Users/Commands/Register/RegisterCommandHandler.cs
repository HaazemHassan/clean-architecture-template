using AutoMapper;
using ErrorOr;
using MediatR;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Features.Users.Common;
using Template.Application.ServicesContracts.Infrastructure;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<UserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPhoneNumberService _phoneNumberService;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService, IAuthenticationService authenticationService, IPhoneNumberService phoneNumberService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _authenticationService = authenticationService;
            _phoneNumberService = phoneNumberService;
        }

        public async Task<ErrorOr<UserResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var normalizedPhoneNumber = _phoneNumberService.Normalize(request.PhoneNumber!);

            var userToAdd = new DomainUser(request.FirstName, request.LastName, request.Email, normalizedPhoneNumber, request.Address);
            var addUserResult = await _applicationUserService.AddUser(userToAdd, request.Password, ct: cancellationToken);

            if (addUserResult.IsError)
                return addUserResult.Errors;

            var userResponse = _mapper.Map<UserResponse>(addUserResult.Value);
            return userResponse;
        }
    }
}
