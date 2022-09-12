using System.ComponentModel.DataAnnotations.Schema;

namespace DCoimbra.Shared;

public interface IEntityBase
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}

public abstract class EntityBase<TKey> : IComparable<EntityBase<TKey>>, IEntityBase
    where TKey : IComparable
{
    /// <summary>
    /// Warning: 
    /// 1. Domain events, normalmente, pertence ao AggregateRoot e não a Entity.
    /// 2. _domainEvents deve ser ignorado no mapeado da persistência. Adicionei o Atributo, porém seria interessante criar uma Strategy de mapeamento a nível do OnCreating do Context.
    /// </summary>
    [NotMapped]
    public List<IDomainEvent> _domainEvents { get; private set; }
    protected EntityBase() => _domainEvents = new List<IDomainEvent>();
    protected EntityBase(TKey id) : this() => Id = id;
    public virtual TKey Id { get; protected set; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();

    public override bool Equals(object obj)
    {
        var compareTo = obj as EntityBase<TKey>;

        if (ReferenceEquals(this, compareTo)) return true;
        if (ReferenceEquals(null, compareTo)) return false;
        if (ReferenceEquals(null, Id)) return false;

        return $"{GetType().Name}.{Id}".Equals($"{compareTo.GetType().Name}.{compareTo.Id}");
    }

    public static bool operator ==(EntityBase<TKey> a, EntityBase<TKey> b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(EntityBase<TKey> a, EntityBase<TKey> b)
        => !(a == b);

    public override int GetHashCode()
        => (GetType().GetHashCode() * 907) + Id.GetHashCode();

    public override string ToString()
        => $"{GetType().Name} [Id={Id}]";

    public virtual int CompareTo(EntityBase<TKey> other)
        => Id.CompareTo(other.Id);
}

public abstract class EntityBase : EntityBase<Guid>
{
    protected EntityBase() : base() { }
    protected EntityBase(Guid id) : base(id) { }
}