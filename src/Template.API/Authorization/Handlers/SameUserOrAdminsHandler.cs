using Microsoft.AspNetCore.Authorization;
using Template.API.Authorization.Requirements;
using Template.Application.Contracts.Services.Api;
using Template.Domain.Enums;

namespace Template.API.Authorization.Handlers;

public class SameUserOrAdminHandler
    : AuthorizationHandler<SameUserOrAdminRequirement, int> {
    private readonly ICurrentUserService _currentUserService;

    public SameUserOrAdminHandler(ICurrentUserService currentUserService) {
        _currentUserService = currentUserService;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SameUserOrAdminRequirement requirement,
        int requestedUserId) {
        if (!_currentUserService.IsAuthenticated)
            return Task.CompletedTask;

        if (_currentUserService.IsInRole(UserRole.SuperAdmin) || _currentUserService.IsInRole(UserRole.Admin)) {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (_currentUserService.UserId == requestedUserId) {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}