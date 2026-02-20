namespace Template.Domain.Entities.Contracts {
    public interface IHasModificationTime {
        DateTime? UpdatedAt { get; set; }
    }
}
