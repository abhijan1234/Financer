using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Financer.DataAccess;
using Financer.DataAccess.Entities.Outbox;
using Financer.DataAccess.Services.DatabaseService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Financer.OutboxEventService
{
    public class OutboxWorker : BackgroundService
    {
        private readonly ILogger<OutboxWorker> _logger;
        private readonly IMongoService _mongoService;
        private readonly IConnection _connection;

        public OutboxWorker(ILogger<OutboxWorker> logger, IMongoService mongoService,IConnection connection)
        {
            _logger = logger;
            _mongoService = mongoService;
            _connection = connection;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("OutboxWorker running at: {time}", DateTimeOffset.Now);
                var allOutBoxData = await _mongoService.GetAllAsync<OutboxEvent>(Constants.MongoInfo.OutboxCollection);

                var result = allOutBoxData.FindAll( data => data.InProcessed == false);

                using var channel = _connection.CreateModel();
                channel.QueueDeclare(Constants.EventInfo.JobCreateEvent,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                foreach(var item in result)
                {
                    var eventMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item.Content.JobInfo));

                    channel.BasicPublish("", Constants.EventInfo.JobCreateEvent,null, eventMessage);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
