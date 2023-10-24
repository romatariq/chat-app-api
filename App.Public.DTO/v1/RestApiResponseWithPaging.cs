﻿namespace App.Public.DTO.v1;

public class RestApiResponseWithPaging<T>
{
    public int PageNr { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public T Data { get; set; } = default!;
}