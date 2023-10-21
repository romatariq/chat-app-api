using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class WebDomain: DomainEntityId
{
    [MaxLength(300)]
    public string Name { get; set; } = default!;


    public ICollection<Url>? Urls { get; set; }

    public ICollection<DomainReport>? DomainReports { get; set; }
}