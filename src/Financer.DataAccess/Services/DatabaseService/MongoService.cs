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

        public async Task UpdateAync<T>(string collectionName, string id,object value, Dictionary<string, object> updateData, IClientSessionHandle? clientSession = null)
        {
            var collection = _mongoDatabase.GetCollection<T>(collectionName);
            var updateDefinition = Builders<T>.Update.Combine(
                updateData.Select(field => Builders<T>.Update.Set(field.Key, field.Value))
                );

            var options = new FindOneAndUpdateOptions<T>
            {
                ReturnDocument = ReturnDocument.Before // Retrieve the document before the update
            };

            var filter = Builders<T>.Filter.Eq(id, value);

            // This will handle atomicity while updating the records even if there are multiple replicas
            var result = await collection.FindOneAndUpdateAsync(clientSession, filter, updateDefinition, options);
            if(result == null)
            {
                throw new InvalidOperationException($"Failed to update item with {id} : {value}");
            }
        }
    }
}
