using Library.Client.Models.Interfaces;
using System.Net.Http.Json;

namespace Library.Client.Services;

public class BaseApiService<TGet, TEdit>(HttpClient http, string endpoint) : IBaseApiService<TGet, TEdit>
{
    public async Task<List<TGet>> GetAllAsync()
    {
        try
        {
            var result = await http.GetFromJsonAsync<List<TGet>>($"api/{endpoint}");
            return result ?? [];
        }
        catch (HttpRequestException)
        {
            return [];
        }
    }

    public async Task<TGet?> GetAsync(int id)
    {
        try
        {
            return await http.GetFromJsonAsync<TGet>($"api/{endpoint}/{id}");
        }
        catch (HttpRequestException)
        {
            return default;
        }
    }

    public async Task<List<TGet>> GetAllAsync(string route)
    {
        try
        {
            var response = await http.GetAsync(route);
            if (!response.IsSuccessStatusCode)
            {
                return [];
            }
            return await response.Content.ReadFromJsonAsync<List<TGet>>() ?? [];
        }
        catch
        {
            return [];
        }
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
