using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using Base.Domain;

namespace App.Domain;

public class Group: DomainEntityId
{
    // If Type is domain, name should match given (email) domain
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    public EGroupType GroupType { get; set; }


    public ICollection<Comment>? Comments { get; set; }

    public ICollection<GroupUser>? GroupUsers { get; set; }
}