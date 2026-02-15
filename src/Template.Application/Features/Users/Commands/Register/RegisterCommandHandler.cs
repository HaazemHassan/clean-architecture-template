using AutoMapper;
using MediatR;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Application.Features.Authentication.Common;
using Template.Domain.Abstracts.RepositoriesAbstracts;
using Template.Domain.Entities;

namespace Template.Application.Features.Users.Commands.Register {
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
            var userToAdd = new DomainUser(request.FirstName, request.LastName, request.Email, request.PhoneNumber, request.Address);
            ServiceOperationResult<DomainUser> addUserResult;
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            {
                addUserResult = await _applicationUserService.AddUser(userToAdd, request.Password, ct: cancellationToken);
                if (!addUserResult.Succeeded)
                    return FromServiceResult<AuthResult>(addUserResult);

                await transaction.CommitAsync(cancellationToken);

            }

            var user = addUserResult.Data;

            var authResult = await _authenticationService.SignInWithPassword(user.Email, request.Password, cancellationToken);
            if (!authResult.Succeeded)
                return FromServiceResult(authResult);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return FromServiceResult(authResult);
        }
    }
}
