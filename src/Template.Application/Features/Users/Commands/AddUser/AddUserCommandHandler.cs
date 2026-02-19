using AutoMapper;
using ErrorOr;
using MediatR;
using Template.Application.Contracts.Infrastructure;
using Template.Domain.Contracts.Repositories;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, ErrorOr<AddUserCommandResponse>>
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

        public async Task<ErrorOr<AddUserCommandResponse>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var domainUser = new DomainUser(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Address);
            var addUserResult = await _applicationUserService.AddUser(domainUser, request.Password, request.UserRole, cancellationToken);

            if (addUserResult.IsError)
                return addUserResult.Errors;

            var response = _mapper.Map<AddUserCommandResponse>(addUserResult.Value);
            return response;
        }
    }
}
