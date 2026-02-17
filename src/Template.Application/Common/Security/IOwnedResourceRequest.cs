namespace Template.Application.Common.Security
{
    public interface IOwnedResourceRequest : IAuthorizedRequest
    {
        public int OwnerUserId { get; }
    }
}
