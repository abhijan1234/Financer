using System;
using RabbitMQ.Client;

namespace Financer.Infrastructure.Factories
{
    public class RabbitMqConnectionFactory
    {
        public readonly string _connectionUrl;

        public RabbitMqConnectionFactory(string connectionUrl)
        {
            _connectionUrl = connectionUrl;
        }

        public IConnection CreateConnection()
        {
            var ConnectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_connectionUrl)
            };
            return ConnectionFactory.CreateConnection();
        }
    }
}
