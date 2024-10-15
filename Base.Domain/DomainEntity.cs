using Base.Contracts.Domain;

namespace Base.Domain;

public abstract class DomainEntity : DomainEntityId, IDomainEntity
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}