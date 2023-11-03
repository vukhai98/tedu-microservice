using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Infrastructure.Extensions;
using MongoDB.Driver;
using Shared.Configurations;
using System.Linq.Expressions;

namespace Contracts.Domains
{
    public class MongoDbRepository<T> : IMogoDbRepositoryBase<T> where T : MongoEntity
    {
        private IMongoDatabase Database { get; }
        public MongoDbRepository(IMongoClient client, MongoDBSettings settings)
        {
            Database = client.GetDatabase(settings.DatabaseName) ?? throw new ArgumentNullException(" Mongo Database is not connect.");
        }
        public Task CreatAsync(T entity)
        {
            return Collection.InsertOneAsync(entity);
        }

        public Task AddRangeAsync(List<T> entities)
        {
            return Collection.InsertManyAsync(entities);
        }

        public Task DeleteAsync(string id)
        {
            return Collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public IMongoCollection<T> FindAll(ReadPreference? readPreference = null)
        {
            return Database.WithReadPreference(readPreference ?? ReadPreference.Primary)
                    .GetCollection<T>(GetCollectionName());
        }

        protected virtual IMongoCollection<T> Collection => Database.GetCollection<T>(GetCollectionName());

        public Task UpdateAsync(T entity)
        {
            Expression<Func<T, string>> func = f => f.Id;

            var value = (string)entity.GetType()
                                      .GetProperty(func.Body.ToString().Split(".")[1])?
                                      .GetValue(entity, null);
            var filter = Builders<T>.Filter.Eq(func, value);

            return Collection.ReplaceOneAsync(filter, entity);
        }

        private static string GetCollectionName()
        {
            return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
        }
    }
}
