using System;
using RabbitMQ.Client;

namespace Financer.Infrastructure.Factories
{
    public class RabbitMqConnectionFactory
    {
        public readonly string _hostName;
        public readonly int _port;
        public readonly string _userName;
        public readonly string _password;

        public RabbitMqConnectionFactory(
            string hostName,
            int port,
            string userName,
            string password
            )
        {
            _hostName = hostName;
            _port = port;
            _userName = userName;
            _password = password;
        }

        public IConnection CreateConnection()
        {
            var ConnectionFactory = new ConnectionFactory
            {
                HostName = _hostName,
                Port = _port,
                UserName = _userName,
                Password = _password,
            };
            return ConnectionFactory.CreateConnection();
        }
    }
}
