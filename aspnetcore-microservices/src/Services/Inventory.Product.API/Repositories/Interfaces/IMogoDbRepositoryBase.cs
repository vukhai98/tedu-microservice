using Inventory.Product.API.Entities.Abstraction;
using MongoDB.Driver;

namespace Inventory.Product.API.Repositories.Interfaces
{
    public interface IMogoDbRepositoryBase<T> where T : MongoEntity
    {
        IMongoCollection<T> FindAll(ReadPreference? readPreference = null);

        Task CreatAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(string id);
    }
}
