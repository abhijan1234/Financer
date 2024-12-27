using System;
using System.Threading.Tasks;

namespace Financer.DataAccess.Services
{
    public interface IMongoService
    {
        Task InsertAync<T>(T data, string collectionName);
    }
}
