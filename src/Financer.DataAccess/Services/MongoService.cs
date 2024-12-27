using MongoDB.Driver;
using System.Threading.Tasks;

namespace Financer.DataAccess.Services
{
    public class MongoService : IMongoService
    {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoService(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public async Task InsertAync<T>(T data, string collectionName)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(data);
        }
    }
}
