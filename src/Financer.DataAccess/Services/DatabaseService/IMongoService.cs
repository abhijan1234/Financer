using System.Threading.Tasks;
using MongoDB.Driver;

namespace Financer.DataAccess.Services.DatabaseService
{
    public interface IMongoService
    {
        Task InsertAync<T>(T data, string collectionName, IClientSessionHandle? clientSessionHandle = null);
    }
}
