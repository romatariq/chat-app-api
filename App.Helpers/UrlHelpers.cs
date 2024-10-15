using System.Text;
using System.Web;

namespace App.Helpers;

public static class UrlHelpers
{
    public static (string domain, string? path, string? parameters) ParseEncodedUrl(string url)
    {
        var (domain, path, parameters) = url.DecodeUrl().SplitUrl();
        return (domain, path.ParsePath(), parameters.ParseParameters());
    }

    private static string DecodeUrl(this string url)
    {
        return HttpUtility.UrlDecode(url);
    }
    
    private static (string domain, string path, string parameters) SplitUrl(this string url)
    {
        var uri = new Uri(url);

        // var domain = uri.Host;
        var domain = uri.Authority;
        var path = uri.LocalPath;
        var parameters = uri.Query;

        return (domain, path, parameters);
    }

    private static string? ParseParameters(this string parameters)
    {
        var paramCollection = HttpUtility.ParseQueryString(parameters);

        var orderedParams = new SortedDictionary<string, string>();

        foreach (var key in paramCollection.AllKeys)
        {
            if (key == null || paramCollection[key] == null) continue;
            orderedParams[key] = paramCollection[key]!;
        }

        if (orderedParams.Count == 0) return null;
        
        var orderedUrlBuilder = new StringBuilder();

        foreach (var kvp in orderedParams)
        {
            orderedUrlBuilder.Append($"&{kvp.Key}={kvp.Value}");
        }

        return orderedUrlBuilder.ToString().TrimStart('&');
    }
    
    private static string? ParsePath(this string path)
    {
        return path == "/" ? null : path.TrimEnd('/');
    }

}