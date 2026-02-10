namespace Template.Core.Entities.Abstracts {
    public interface IHasCreator {
        int? CreatedBy { get; set; }
    }
}
