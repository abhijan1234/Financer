using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financer.DataAccess.Services.DatabaseService
{
    public class MongoService : IMongoService
    {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoService(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public async Task InsertAync<T>(T data, string collectionName, IClientSessionHandle? clientSession = null)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(clientSession,data);
        }

        public async Task<List<T>> GetAllAsync<T>(string collectionName)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            return await collection.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAync<T>(string collectionName, string id,object value, Dictionary<string, object> updateData)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            var updateDefinition = Builders<T>.Update.Combine(
                updateData.Select(field => Builders<T>.Update.Set(field.Key, field.Value))
                );

            var filter = Builders<T>.Filter.Eq(id, value);
            var result = await collection.UpdateOneAsync(filter, updateDefinition);
            if(result.MatchedCount == 0)
            {
                throw new InvalidOperationException($"No item matched with {id} : {value}");
            }
        }
    }
}
