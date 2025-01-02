﻿using MongoDB.Driver;
using System.Collections.Generic;
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
    }
}
