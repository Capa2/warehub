
namespace warehub.db
{
    public interface ICRUDService
    {
        bool Create(string table, Dictionary<string, object> parameters);
        bool Delete(string table, string idColumn, object idValue);
        (bool, List<Dictionary<string, object>>) Read(string table, Dictionary<string, object> parameters);
        bool Update(string table, Dictionary<string, object> parameters, string idColumn, object idValue);
    }
}