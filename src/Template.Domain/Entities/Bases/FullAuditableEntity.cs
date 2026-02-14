using Template.Domain.Common.Auditing;

namespace Template.Domain.Entities.Bases {

    public abstract class FullAuditableEntity<TId> : AuditableEntity<TId>, IFullyAuditableEntity {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }
}
