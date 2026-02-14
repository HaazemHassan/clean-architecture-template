namespace Template.Domain.Common.Auditing {
    public interface ISoftDeletableEntity {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        int? DeletedBy { get; set; }
    }
}
