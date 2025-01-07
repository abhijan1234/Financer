using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Financer.DataAccess;
using Financer.DataAccess.Entities.Jobs;
using Financer.DataAccess.Entities.Outbox;
using Financer.DataAccess.Services.DatabaseService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Financer.OutboxEventService
{
    public class OutboxWorker : BackgroundService
    {
        private readonly ILogger<OutboxWorker> _logger;
        private readonly IMongoService _mongoService;
        private readonly IConnection _connection;
        private readonly IMongoDatabase _mongoDatabase;

        public OutboxWorker(ILogger<OutboxWorker> logger,
            IMongoService mongoService,
            IConnection connection,
            IMongoDatabase mongoDatabase
            )
        {
            _logger = logger;
            _mongoService = mongoService;
            _connection = connection;
            _mongoDatabase = mongoDatabase;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("OutboxWorker running at: {time}", DateTimeOffset.Now);

                // Reading all unprocessed data

                var allOutBoxData = await _mongoService.GetAllAsync<OutboxEvent>(Constants.MongoInfo.OutboxCollection);

                var result = allOutBoxData.FindAll( data => data.IsSent == false &&
                data.Content.JobStatus == Constants.JobStatus.Created);

                using var channel = _connection.CreateModel();
                channel.QueueDeclare(Constants.EventInfo.JobCreateEvent,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                // Sending all unprocessed data to the queue

                foreach(var item in result)
                {
                    try
                    {
                        var eventMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item.Content.JobInfo));
                        channel.BasicPublish("", Constants.EventInfo.JobCreateEvent, null, eventMessage);

                        // Updating Outbox table IsSent and Job table JobStatus in one transaction

                        using var session = await _mongoDatabase.Client.StartSessionAsync();
                        {
                            session.StartTransaction();
                            try
                            {
                                var outBoxUpdateClause = new Dictionary<string, object>
                                {
                                    {"IsSent", true}
                                };
                                await _mongoService.UpdateAync<OutboxEvent>(
                                    Constants.MongoInfo.OutboxCollection,
                                    "Content.JobId",
                                    item.Content.JobId,
                                    outBoxUpdateClause,
                                    session
                                    );

                                var jobUpdateClause = new Dictionary<string, object>
                                {
                                    { "JobStatus", Constants.JobStatus.InProgress }
                                };

                                await _mongoService.UpdateAync<Job>(
                                    Constants.MongoInfo.JobCollection,
                                    "JobId",
                                    item.Content.JobId,
                                    jobUpdateClause,
                                    session
                                    );
                                await session.CommitTransactionAsync();
                            }
                            catch(Exception ex)
                            {
                                await session.AbortTransactionAsync();
                                _logger.LogError($"Transaction failed with this error message: {ex.Message}");
                                throw ex;
                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
