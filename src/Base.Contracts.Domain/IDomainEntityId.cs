namespace Base.Contracts.Domain;

public interface IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
{
    TKey Id { get; set; }
}

public interface IDomainEntityId : IDomainEntityId<Guid>
{
}