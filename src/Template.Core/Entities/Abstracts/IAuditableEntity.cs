namespace Template.Core.Entities.Abstracts {
    public interface IAuditableEntity : IHasCreationTime, IHasCreator, IHasModificationTime, IHasModifier {

    }
}
