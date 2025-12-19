namespace Library.Client.Models.Interfaces;

/// <summary>
/// Generic interface defining standard CRUD operations for API services.
/// </summary>
/// <typeparam name="TGet">The type used for retrieving entities (DTO for GET operations).</typeparam>
/// <typeparam name="TEdit">The type used for creating or updating entities (DTO for POST/PUT operations).</typeparam>
public interface IBaseApiService<TGet, TEdit>
{
    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>A list of <typeparamref name="TGet"/> representing all entities.</returns>
    public Task<List<TGet>> GetAllAsync();

    /// <summary>
    /// Retrieves a single entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity as <typeparamref name="TGet"/> if found; otherwise, null.</returns>
    public Task<TGet?> GetAsync(int id);

    /// <summary>
    /// Retrieves all entities from a specific API route.
    /// </summary>
    /// <param name="route">The API route to fetch the entities from.</param>
    /// <returns>A list of <typeparamref name="TGet"/> from the specified route.</returns>
    public Task<List<TGet>> GetAllAsync(string route);

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="dto">The data transfer object containing information for the new entity.</param>
    public Task CreateAsync(TEdit dto);

    /// <summary>
    /// Updates an existing entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to update.</param>
    /// <param name="dto">The data transfer object containing updated information.</param>
    public Task UpdateAsync(int id, TEdit dto);

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    public Task DeleteAsync(int id);
}
