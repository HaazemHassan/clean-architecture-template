namespace Template.Core.Entities.Abstracts {
    public interface IHasModificationTime {
        DateTime? UpdatedAt { get; set; }
    }
}
