using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Url: DomainEntityId
{
    [MaxLength(1000)]
    public string? Path { get; set; }

    [MaxLength(1000)]
    public string? Params { get; set; }

    
    public Guid WebDomainId { get; set; }
    public WebDomain? WebDomain { get; set; }

    public ICollection<Comment>? Comments { get; set; }
}