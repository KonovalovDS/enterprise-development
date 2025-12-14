namespace Library.Client.Models.Interfaces;

public interface ITokenStorage
{
    public Task SetTokenAsync(string token);
    public Task<string?> GetTokenAsync();
    public Task RemoveTokenAsync();
}
