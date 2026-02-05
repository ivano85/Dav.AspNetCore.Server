namespace Dav.AspNetCore.Server;

internal static class UriHelper
{
    public static string GetPath(this Uri uri)
    {
        // Estrai il percorso da un URI che può essere relativo o assoluto
        ArgumentNullException.ThrowIfNull(uri, nameof(uri));
        return uri.IsAbsoluteUri ? uri.AbsolutePath : uri.OriginalString;
    }

    public static Uri GetParent(this Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri, nameof(uri));

        // Normalizza l'URI per assicurarti che sia in formato relativo
        var path = uri.IsAbsoluteUri ? uri.LocalPath : uri.OriginalString;
        path = path.TrimEnd('/');

        if (string.IsNullOrEmpty(path) || path == "/")
            return new Uri("/", UriKind.Relative);

        var lastSlash = path.LastIndexOf('/');
        if (lastSlash <= 0)
            return new Uri("/", UriKind.Relative);

        var parentPath = path.Substring(0, lastSlash);
        if (string.IsNullOrEmpty(parentPath))
            parentPath = "/";

        return new Uri(parentPath, UriKind.Relative);
    }

    public static Uri Combine(Uri uri, string path)
    {
        var localPath = uri.IsAbsoluteUri ? uri.LocalPath : uri.OriginalString;
        if (!localPath.EndsWith("/"))
            localPath += "/";

        var combinedPath = $"{localPath}{path.TrimStart('/')}";
        return new Uri(combinedPath, UriKind.Relative);
    }

    public static Uri GetRelativeUri(this Uri relativeTo, Uri uri)
    {
        ArgumentNullException.ThrowIfNull(relativeTo, nameof(relativeTo));
        ArgumentNullException.ThrowIfNull(uri, nameof(uri));
        
        var relativeToPath = relativeTo.IsAbsoluteUri ? relativeTo.LocalPath : relativeTo.OriginalString;
        var uriPath = uri.IsAbsoluteUri ? uri.LocalPath : uri.OriginalString;

        relativeToPath = relativeToPath.TrimEnd('/');
        uriPath = uriPath.TrimEnd('/');

        if (!relativeToPath.StartsWith(uriPath))
            return uri;

        if (relativeToPath.Length == uriPath.Length)
            return new Uri("/", UriKind.Relative);

        var relativePath = relativeToPath.Substring(uriPath.Length);
        if (!relativePath.StartsWith("/"))
            relativePath = "/" + relativePath;

        return new Uri(relativePath, UriKind.Relative);
    }
}