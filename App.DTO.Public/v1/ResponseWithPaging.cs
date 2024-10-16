namespace App.DTO.Public.v1;

public class ResponseWithPaging<T>
{
    public int PageNr { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public IEnumerable<T> Data { get; set; } = default!;
}