using App.Domain.Enums;
using Base.Contracts.Domain;

namespace App.DTO.Private.DAL;

public class Group: IDomainEntityId
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public EGroupType Type { get; set; }

    public bool IsOwner { get; set; }
}