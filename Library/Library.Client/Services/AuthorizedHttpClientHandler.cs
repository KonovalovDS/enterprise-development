using Library.Client.Models.Interfaces;
using System.Net.Http.Headers;

namespace Library.Client.Services;

/// <summary>
/// HTTP message handler that automatically adds a Bearer JWT token
/// to outgoing HTTP requests if a token is available in <see cref="ITokenStorage"/>.
/// Inherits from <see cref="DelegatingHandler"/>.
/// </summary>
/// <param name="tokenStorage">The <see cref="ITokenStorage"/> used to retrieve the JWT token.</param>
public class AuthorizedHttpClientHandler(ITokenStorage tokenStorage) : DelegatingHandler
{
    /// <summary>
    /// Sends an HTTP request asynchronously, adding the Authorization header
    /// with the Bearer token if a token exists.
    /// </summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await tokenStorage.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
