using System.Net;

namespace App.DTO.Public.v1;
public class ErrorResponse(HttpStatusCode statusCode, string message)
{
    public HttpStatusCode StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message;
}
