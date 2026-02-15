using Microsoft.AspNetCore.Identity;
using Template.Application.Common.Responses;
using Template.Application.Contracts.Services.Api;
using Template.Application.Contracts.Services.Infrastructure;
using Template.Application.Enums;
using Template.Domain.Abstracts.RepositoriesAbstracts;
using Template.Domain.Entities;
using Template.Domain.Enums;
using Template.Infrastructure.Common;
using Template.Infrastructure.Data.IdentityEntities;
using Template.Infrastructure.Extensions;

namespace Template.Infrastructure.Services {
    internal class ApplicationUserService : IApplicationUserService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientContextService _clientContextService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;


        public ApplicationUserService(IUnitOfWork unitOfWork, IClientContextService clientContextService, ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager) {
            _unitOfWork = unitOfWork;
            _clientContextService = clientContextService;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<ServiceOperationResult<DomainUser>> AddUser(DomainUser user, string password, UserRole role = UserRole.User, CancellationToken ct = default) {
            var userByPhone = await _unitOfWork.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber, ct);

            if (userByPhone)
                return ServiceOperationResult<DomainUser>
                    .Failure(ServiceOperationStatus.AlreadyExists, "Phone number already exists");

            var applicationUser = new ApplicationUser(user.Email, user.PhoneNumber);
            applicationUser.AssignDomainUser(user);

            var createResult = await _userManager.CreateAsync(applicationUser, password);

            if (!createResult.Succeeded) {
                if (createResult.HasError(IdentityErrorCodes.DuplicateEmail))
                    return ServiceOperationResult<DomainUser>
                          .Failure(ServiceOperationStatus.AlreadyExists, "This Email already exists");

                return ServiceOperationResult<DomainUser>
                           .Failure(ServiceOperationStatus.Failed, "Failed to create user. Please try again later.");
            }


            var addToRoleResult = await _userManager.AddToRoleAsync(applicationUser, role.ToString()!);
            if (!addToRoleResult.Succeeded)
                return ServiceOperationResult<DomainUser>.
                    Failure(ServiceOperationStatus.Failed, "Failed to create user. Please try again later.");


            return ServiceOperationResult<DomainUser>.Success(user, ServiceOperationStatus.Created);
        }

    }
}
