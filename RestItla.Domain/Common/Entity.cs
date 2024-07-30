namespace RestItla.Domain.Common
{
    public class Entity<TKey> : IEquatable<Entity<TKey>>
    where TKey : struct
    {
        public TKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool Equals(Entity<TKey>? other)
        {
            return other?.Equals(Id) == true;
        }
    }

    public class Entity
    : Entity<Guid>
    {
    }
}