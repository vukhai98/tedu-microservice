using Contracts.Domains;
using MongoDB.Driver;

namespace Contracts.Domains.Interfaces
{
    public interface IMogoDbRepositoryBase<T> where T : MongoEntity
    {
        IMongoCollection<T> FindAll(ReadPreference? readPreference = null);

        Task CreatAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(string id);
    }
}
