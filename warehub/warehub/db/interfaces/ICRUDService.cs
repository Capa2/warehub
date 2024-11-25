namespace warehub.db.interfaces
{
    public interface ICRUDService
    {
        Task<bool> Create(string table, Dictionary<string, object> parameters);
        Task<bool> Delete(string table, string idColumn, object idValue);
        Task<(bool, List<Dictionary<string, object>>)> Read(string table, Dictionary<string, object> parameters);
        Task<bool> Update(string table, Dictionary<string, object> parameters, string idColumn, object idValue);
    }
}