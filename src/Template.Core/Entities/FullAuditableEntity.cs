using Template.Core.Entities.Abstracts;

namespace Template.Core.Entities {

    public abstract class FullAuditableEntity<TId> : AuditableEntity<TId>, IFullyAuditableEntity {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }
}
