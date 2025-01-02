using System;
using Financer.API.Config;
using Financer.DataAccess.Services.DatabaseService;
using Financer.Infrastructure.Factories;
using Financer.Infrastructure.Repository.JobRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

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
            //var rabbitmqConnection= Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();
            //if(string.IsNullOrEmpty(rabbitmqConnection.HostName) ||
            //    string.IsNullOrEmpty(rabbitmqConnection.Port.ToString()) ||
            //    string.IsNullOrEmpty(rabbitmqConnection.UserName) ||
            //    string.IsNullOrEmpty(rabbitmqConnection.Password))
            //{
            //    throw new Exception("No RabbitMQ connection could not be created. Missing configuration");
            //}
            //services.AddSingleton(new RabbitMqConnectionFactory(
            //    rabbitmqConnection.HostName,
            //    rabbitmqConnection.Port,
            //    rabbitmqConnection.UserName,
            //    rabbitmqConnection.Password
            //    ).InitializeConnection());

            // Dependency Injection of services
            services.AddSingleton<IMongoService,MongoService>();
            services.AddScoped<ICreateJobRepository, CreateJobRepository>();
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
