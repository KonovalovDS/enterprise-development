namespace Library.Client.Models.Interfaces;

public interface IBaseApiService<TGet, TEdit>
{
    public Task<List<TGet>> GetAllAsync();
    public Task<TGet?> GetAsync(int id);
    public Task<List<TGet>> GetAllAsync(string route);
    public Task CreateAsync(TEdit dto);
    public Task UpdateAsync(int id, TEdit dto);
    public Task DeleteAsync(int id);
}
