using Template.Application.Common.Responses;

namespace Template.Application.Contracts.Services.Infrastructure
{
    public interface IPolicyEnforcer
    {
        ServiceOperationResult Authorize<TRequest>(
            TRequest request,
            int userId,
            string policyName);
    }
}
