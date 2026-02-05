using Microsoft.AspNetCore.Http;

namespace Dav.AspNetCore.Server.Store;

public interface IRedirectableStoreItem
{
    Task<Uri?> GetRedirectUriAsync(HttpContext context, CancellationToken cancellationToken = default);
}
