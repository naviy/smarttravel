namespace Luxena.Base.Domain
{


    public class Entity<TId>
    {

        public virtual TId Id { get; set; }
        public virtual int Version { get; set; }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            var entity = obj as Entity<TId>;
            return entity != null && (ReferenceEquals(this, entity) || Equals(Id, entity.Id));
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }


}