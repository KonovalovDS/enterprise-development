using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Library.Client.Services;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous =
        new(new ClaimsIdentity());

    private ClaimsPrincipal _currentUser =
        new(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentUser));
    }

    public void SetUser(ClaimsPrincipal user)
    {
        _currentUser = user;
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(user)));
    }

    public void SetAnonymous()
    {
        _currentUser = _anonymous;
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(_anonymous)));
    }
}
