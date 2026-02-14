namespace Template.Domain.Common.Auditing {
    public interface IHasCreator {
        int? CreatedBy { get; set; }
    }
}
