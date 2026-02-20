namespace Template.Domain.Entities.Contracts {
    public interface ISoftDeletableEntity {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        int? DeletedBy { get; set; }
    }
}
