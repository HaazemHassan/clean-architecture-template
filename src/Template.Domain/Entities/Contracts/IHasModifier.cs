namespace Template.Domain.Entities.Contracts {
    public interface IHasModifier {
        int? UpdatedBy { get; set; }
    }
}
