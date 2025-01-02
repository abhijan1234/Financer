using System;
namespace Financer.API.Config
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
