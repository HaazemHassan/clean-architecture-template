namespace Template.Application.Contracts.Requests
{

    // Used especialy for selfOrAdmin policy
    public interface IOwnedResourceRequest : IAuthorizedRequest
    {
        public int OwnerUserId { get; }
    }
}
