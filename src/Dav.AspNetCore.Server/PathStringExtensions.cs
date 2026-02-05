using Microsoft.AspNetCore.Http;

namespace Dav.AspNetCore.Server;

internal static class PathStringExtensions
{
    public static Uri ToUri(this PathString path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return new Uri("/", UriKind.Relative);

        // Normalizza il percorso: sostituisci backslash con forward slash (per Windows)
        var normalized = path.ToUriComponent().Replace("\\", "/").TrimEnd('/');
        var uri = Uri.UnescapeDataString(normalized);
        
        if (string.IsNullOrWhiteSpace(uri))
            return new Uri("/", UriKind.Relative);

        // Assicurati che inizi con /
        if (!uri.StartsWith("/"))
            uri = "/" + uri;

        try
        {
            return new Uri(uri, UriKind.Relative);
        }
        catch (UriFormatException)
        {
            // Se l'URI è ancora invalido, ritorna il percorso root
            return new Uri("/", UriKind.Relative);
        }
    }
}