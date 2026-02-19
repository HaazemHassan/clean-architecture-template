using ErrorOr;
using Template.Application.Security.Contracts;
using Template.Application.Security.Policies;
using Template.Domain.Common.Constants;

namespace Template.Infrastructure.Security
{
    internal class PolicyEnforcer : IPolicyEnforcer
    {
        private readonly IEnumerable<IAuthorizationPolicy> _policies;

        public PolicyEnforcer(IEnumerable<IAuthorizationPolicy> policies)
        {
            _policies = policies;
        }
        public ErrorOr<Success> Authorize<TRequest>(TRequest request, string policyName)
        {
            var policy = _policies.FirstOrDefault(p => p.Name == policyName);

            if (policy is null)
            {
                return Error.Failure(
                    code: ErrorCodes.Authorization.UnknownPolicy,
                    description: $"Unknown policy: {policyName}");
            }

            return policy.Authorize(request!);
        }


    }
}
