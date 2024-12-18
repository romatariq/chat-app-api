namespace Base.Contracts.Domain;

public interface IDomainEntity : IDomainEntityId
{
    DateTime CreatedAtUtc { get; set; }
}