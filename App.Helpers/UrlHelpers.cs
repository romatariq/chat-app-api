using System.Text;
using System.Web;
using App.Domain.Exceptions;

namespace App.Helpers;

public static class UrlHelpers
{
    public static (string domain, string? path, string? parameters) ParseEncodedUrl(string url)
    {
        var parts = url.DecodeUrl().SplitUrl();
        return (parts.domain.ParseDomain(), parts.path.ParsePath(), parts.parameters.ParseParameters());
    }

    private static string DecodeUrl(this string url)
    {
        return HttpUtility.UrlDecode(url);
    }
    
    private static (string domain, string path, string parameters) SplitUrl(this string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return (uri.Authority, uri.LocalPath, uri.Query);
        }

        throw new CustomUserBadInputException($"Invalid url: '{url}'");
    }


    private static string ParseDomain(this string domain)
    {
        var lowerDomain = domain.ToLower();
        if (lowerDomain.StartsWith("www.") && lowerDomain.Count(x => x == '.') > 1)
        {
            return lowerDomain[4..];
        }
        return lowerDomain;
    }

    private static string? ParsePath(this string path)
    {
        return path == "/" ? null : path.TrimEnd('/');
    }

    private static string? ParseParameters(this string parameters)
    {
        var orderedParams = new SortedDictionary<string, string>();

        var paramCollection = HttpUtility.ParseQueryString(parameters);
        foreach (var key in paramCollection.AllKeys)
        {
            if (key != null && paramCollection[key] != null)
            {
                orderedParams[key] = paramCollection[key]!;
            }
        }

        if (orderedParams.Count == 0)
        {
            return null;
        }
        
        var orderedUrlBuilder = new StringBuilder();
        foreach (var kvp in orderedParams)
        {
            orderedUrlBuilder.Append($"&{kvp.Key}={kvp.Value}");
        }

        return orderedUrlBuilder.ToString().TrimStart('&');
    }
}