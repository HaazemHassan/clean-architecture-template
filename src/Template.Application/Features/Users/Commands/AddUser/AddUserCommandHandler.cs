using AutoMapper;
using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandHandler : ResultHandler, IRequestHandler<AddUserCommand, Result<AddUserCommandResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;

        public AddUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserService applicationUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
        }

        public async Task<Result<AddUserCommandResponse>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var domainUser = new DomainUser(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Address);
            var addUserResult = await _applicationUserService.AddUser(domainUser, request.Password, request.UserRole, cancellationToken);

            if (!addUserResult.Succeeded)
                return FromServiceResult<AddUserCommandResponse>(addUserResult);

            var response = _mapper.Map<AddUserCommandResponse>(addUserResult.Data);
            return Created(response);
        }
    }
}
