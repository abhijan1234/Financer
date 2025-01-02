using System;
using Financer.API.Config;
using Financer.DataAccess.Services.DatabaseService;
using Financer.Infrastructure.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Financer.OutboxEventService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var Configuration = hostContext.Configuration;

                    var mongoConfig = Configuration.GetSection("Mongo").Get<MongoConfig>();
                    if (string.IsNullOrEmpty(mongoConfig.DbConnectionString))
                    {
                        throw new Exception("No MongoDB connection was found");
                    }
                    services.AddSingleton<IMongoDatabase>(
                        new MongoDbConnectionFactory(mongoConfig.DbConnectionString, mongoConfig.DatabaseName)
                        .InitializeConnection());
                    // Dependency Injection of RabbitMQ
                    var rabbitmqConnection = Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();
                    if (string.IsNullOrEmpty(rabbitmqConnection.HostName) ||
                        string.IsNullOrEmpty(rabbitmqConnection.Port.ToString()) ||
                        string.IsNullOrEmpty(rabbitmqConnection.UserName) ||
                        string.IsNullOrEmpty(rabbitmqConnection.Password))
                    {
                        throw new Exception("No RabbitMQ connection could not be created. Missing configuration");
                    }
                    services.AddSingleton(new RabbitMqConnectionFactory(
                        rabbitmqConnection.HostName,
                        rabbitmqConnection.Port,
                        rabbitmqConnection.UserName,
                        rabbitmqConnection.Password
                        ).InitializeConnection());

                    services.AddSingleton<IMongoService, MongoService>();

                    services.AddHostedService<OutboxWorker>();
                });
    }
}
