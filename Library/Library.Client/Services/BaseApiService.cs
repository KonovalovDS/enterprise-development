using Library.Client.Models.Interfaces;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Library.Client.Services;

public class BaseApiService<TGet, TEdit>(HttpClient http, string endpoint) : IBaseApiService<TGet, TEdit>
{
    public async Task<List<TGet>> GetAllAsync()
    {
        var result = await http.GetFromJsonAsync<List<TGet>>($"api/{endpoint}");
        return result ?? [];
    }

    public async Task<TGet?> GetAsync(int id)
    {
        return await http.GetFromJsonAsync<TGet>($"api/{endpoint}/{id}");
    }

    public async Task<List<TGet>> GetAllAsync(string route)
    {
        var response = await http.GetAsync(route);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<TGet>>() ?? [];
    }

    public async Task CreateAsync(TEdit dto)
    {
        await http.PostAsJsonAsync($"api/{endpoint}", dto);
    }

    public async Task UpdateAsync(int id, TEdit dto)
    {
        await http.PutAsJsonAsync($"api/{endpoint}/{id}", dto);
    }

    public async Task DeleteAsync(int id)
    {
        await http.DeleteAsync($"api/{endpoint}/{id}");
    }
}
