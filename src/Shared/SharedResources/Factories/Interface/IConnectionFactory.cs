using System;
namespace Financer.Infrastructure.Factories.Interface
{
    public interface IConnectionFactory<T>
    {
        T InitializeConnection();
    }
}
