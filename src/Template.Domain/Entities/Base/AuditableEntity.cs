using Template.Domain.Common.Auditing;
using Template.Domain.Primitives.Template.Domain.Primitives;

namespace Template.Domain.Entities.Base
{
    public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity where TId : notnull
    {

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
