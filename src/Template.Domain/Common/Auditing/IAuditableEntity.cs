namespace Template.Domain.Common.Auditing {
    public interface IAuditableEntity : IHasCreationTime, IHasCreator, IHasModificationTime, IHasModifier {

    }
}
