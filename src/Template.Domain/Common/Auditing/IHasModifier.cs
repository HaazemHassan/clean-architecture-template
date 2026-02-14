namespace Template.Domain.Common.Auditing {
    public interface IHasModifier {
        int? UpdatedBy { get; set; }
    }
}
