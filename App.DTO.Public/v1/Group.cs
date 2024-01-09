using App.Domain;

namespace App.DTO.Public.v1;

public class Group
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public EGroupType Type { get; set; }

    public bool IsOwner { get; set; }
}