using App.DTO.Common;

namespace App.DTO.Public.v1;

public class DomainReports
{
    public string Domain { get; set; } = default!;
    public IEnumerable<DomainReport> Reports { get; set; } = default!;
}