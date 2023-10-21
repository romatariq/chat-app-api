using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class GroupUser: DomainEntityId
{
    public bool IsOwner { get; set; }


    public Guid UserId { get; set; }
    public AppUser? User { get; set; }

    public Guid GroupId { get; set; }
    public Group? Group { get; set; }
}