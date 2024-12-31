using System;
using Financer.Infrastructure.Factories.Interface;
using MongoDB.Driver;

namespace Financer.Infrastructure.Factories
{
    public class MongoDbConnectionFactory : IConnectionFactory<IMongoDatabase>
    {
        private readonly string _connectionString;
        private readonly string _databaseName;

        public MongoDbConnectionFactory(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
        }

        public IMongoDatabase InitializeConnection()
        {
            var dbClient = new MongoClient(_connectionString);
            return dbClient.GetDatabase(_databaseName);
        }
    }
}
