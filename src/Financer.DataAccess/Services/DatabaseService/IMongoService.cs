using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Financer.DataAccess.Services.DatabaseService
{
    public interface IMongoService
    {
        Task InsertAync<T>(T data, string collectionName, IClientSessionHandle? clientSessionHandle = null);
        Task<List<T>> GetAllAsync<T>(string collectionName);
        Task UpdateAync<T>(string collectionName, string identifierName, object identifierValue, Dictionary<string, object> updateData, IClientSessionHandle? clientSession = null);
    }
}
