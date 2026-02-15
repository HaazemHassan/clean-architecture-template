using Template.Domain.Common.Auditing;

namespace Template.Domain.Primitives
{

    public abstract class FullAuditableEntity<TId> : AuditableEntity<TId>, IFullyAuditableEntity where TId : notnull
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }
}
