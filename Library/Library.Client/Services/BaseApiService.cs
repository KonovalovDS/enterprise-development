using Library.Client.Models.Interfaces;
using System.Net.Http.Json;

namespace Library.Client.Services;

/// <summary>
/// Generic base service providing standard CRUD operations over HTTP for API endpoints.
/// Implements <see cref="IBaseApiService{TGet, TEdit}"/>.
/// </summary>
/// <typeparam name="TGet">The type used for GET operations (read DTO).</typeparam>
/// <typeparam name="TEdit">The type used for create/update operations (write DTO).</typeparam>
/// <param name="http">The <see cref="HttpClient"/> used to perform HTTP requests.</param>
/// <param name="endpoint">The API endpoint associated with this service (e.g., "books").</param>
public class BaseApiService<TGet, TEdit>(HttpClient http, string endpoint) : IBaseApiService<TGet, TEdit>
{
    /// <summary>
    /// Retrieves all entities from the default API endpoint.
    /// </summary>
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

    /// <summary>
    /// Retrieves a single entity by its ID from the API.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
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

    /// <summary>
    /// Retrieves all entities from a specified API route.
    /// </summary>
    /// <param name="route">The custom API route to fetch entities from.</param>
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

    /// <summary>
    /// Creates a new entity by sending a POST request to the API.
    /// </summary>
    /// <param name="dto">The data transfer object containing information for the new entity.</param>
    public async Task CreateAsync(TEdit dto) =>
        await http.PostAsJsonAsync($"api/{endpoint}", dto);

    /// <summary>
    /// Updates an existing entity by its ID by sending a PUT request to the API.
    /// </summary>
    /// <param name="id">The ID of the entity to update.</param>
    /// <param name="dto">The data transfer object containing updated information.</param>
    public async Task UpdateAsync(int id, TEdit dto)
    {
        await http.PutAsJsonAsync($"api/{endpoint}/{id}", dto);
    }

    /// <summary>
    /// Deletes an entity by its ID by sending a DELETE request to the API.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    public async Task DeleteAsync(int id)
    {
        await http.DeleteAsync($"api/{endpoint}/{id}");
    }
}
