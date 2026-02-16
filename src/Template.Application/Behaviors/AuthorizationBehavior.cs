using MediatR;
using Template.Application.Common.Exceptions;
using Template.Application.Contracts;
using Template.Application.Contracts.Services.Api;
using Template.Application.Contracts.Services.Infrastructure;

namespace Template.Application.Behaviors
{
    public sealed class AuthorizationBehavior<TRequest, TResponse>
      : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IPermissionService _permissionService;

        public AuthorizationBehavior(
            ICurrentUserService currentUser,
            IPermissionService permissionService)
        {
            _currentUserService = currentUser;
            _permissionService = permissionService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is not IAuthorizedRequest authorizedRequest)
                return await next();

            if (_currentUserService.UserId is null || !_currentUserService.IsAuthenticated)
                throw new UnauthorizedException();

            var hasPermission = await _permissionService.HasPermissionsAsync(
                _currentUserService.UserId.Value,
                authorizedRequest.RequiredPermissions,
                cancellationToken);

            if (!hasPermission)
                throw new ForbiddenException();

            return await next();
        }
    }

}
