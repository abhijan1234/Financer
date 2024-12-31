using System;
using Financer.API.Config;
using Financer.DataAccess.Services;
using Financer.Infrastructure.Factories;
using Financer.Infrastructure.Services.JobServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace Financer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen();

            // Dependency Injection of MongoDb
            var mongoConfig = Configuration.GetSection("Mongo").Get<MongoConfig>();
            if(string.IsNullOrEmpty(mongoConfig.DbConnectionString))
            {
                throw new Exception("No MongoDB connection was found");
            }
            services.AddSingleton<IMongoDatabase>(
                new MongoDbConnectionFactory(mongoConfig.DbConnectionString, mongoConfig.DatabaseName)
                .InitializeConnection());

            // Dependency Injection of RabbitMQ
            var rabbitmqConnection= Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();
            if(string.IsNullOrEmpty(rabbitmqConnection.ConnectionUri))
            {
                throw new Exception("No RabbitMQ connection was found");
            }
            //services.AddSingleton(new RabbitMqConnectionFactory(rabbitmqConnection.ConnectionUri).CreateConnection());

            services.AddSingleton<IMongoService,MongoService>();
            services.AddScoped<ICreateJobService, CreateJobService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
