using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class DomainReport: DomainEntityId
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public EReportType ReportType { get; set; }
    
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }

    public Guid WebDomainId { get; set; }
    public WebDomain? WebDomain { get; set; }
}