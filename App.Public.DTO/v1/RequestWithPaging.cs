using System.ComponentModel.DataAnnotations;

namespace App.Public.DTO.v1;

public class RequestWithPaging
{
    [Range(1, int.MaxValue)] public int PageNr { get; set; } = 1;

    [Range(1, 100)] public int PageSize { get; set; } = 25;
}