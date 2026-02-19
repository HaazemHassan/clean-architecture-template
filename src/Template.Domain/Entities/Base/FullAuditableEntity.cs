using Template.Domain.Common.Auditing;

namespace Template.Domain.Entities.Base
{

    public abstract class FullAuditableEntity<TId> : AuditableEntity<TId>, IFullyAuditableEntity where TId : notnull
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }
}
