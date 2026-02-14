namespace Template.Domain.Entities.Bases {
    public abstract class BaseEntity<TId> {
        public TId Id { get; set; }

    }
}
