using Microsoft.AspNetCore.Identity;
using Template.Core.Abstracts.ApiAbstracts;
using Template.Core.Abstracts.InfrastructureAbstracts.Repositories;
using Template.Core.Bases.Responses;
using Template.Core.Entities.IdentityEntities;
using Template.Core.Entities.UserEntities;
using Template.Core.Enums;

namespace Template.Infrastructure.Services {
    public class ApplicationUserService : IApplicationUserService {
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

            if (!_currentUserService.IsInRole(UserRole.Admin))
                return ServiceOperationResult<DomainUser>.Failure(ServiceOperationStatus.Forbidden);
            else if (!_currentUserService.IsAuthenticated)
                role = UserRole.User;

            if (await _unitOfWork.Users.AnyAsync(x => x.Email == user.Email, ct))
                return ServiceOperationResult<DomainUser>.
                    Failure(ServiceOperationStatus.AlreadyExists, "Email already exists.");

            if (await _unitOfWork.Users.AnyAsync(x => x.PhoneNumber == user.PhoneNumber, ct))
                return ServiceOperationResult<DomainUser>.
                    Failure(ServiceOperationStatus.AlreadyExists, "This phone number is used.");


            var applicationUser = ApplicationUser.Create(user.Email, user.PhoneNumber);
            applicationUser.AssignDomainUser(user);

            var createResult = await _userManager.CreateAsync(applicationUser, password);

            if (!createResult.Succeeded)
                return ServiceOperationResult<DomainUser>.
                    Failure(ServiceOperationStatus.Failed, "Failed to create user. Please try again later.");

            var addToRoleResult = await _userManager.AddToRoleAsync(applicationUser, role.ToString()!);
            if (!addToRoleResult.Succeeded)
                return ServiceOperationResult<DomainUser>.
                    Failure(ServiceOperationStatus.Failed, "Failed to create user. Please try again later.");


            //var succedded = await SendConfirmationEmailAsync(applicationUser);
            //if (!succedded)
            //    return ServiceOperationResult.Failed;

            return ServiceOperationResult<DomainUser>.
                Success(user, ServiceOperationStatus.Created);
        }


    }
}
