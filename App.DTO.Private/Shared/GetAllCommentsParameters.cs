using App.DTO.Common;

namespace App.DTO.Private.Shared;

public record GetAllCommentsParameters(
    Guid GroupId,
    Guid? UserId,
    string Domain,
    string? Path,
    string? Parameters,
    ESort Sort,
    int PageNr,
    int PageSize
);