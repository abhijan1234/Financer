using Financer.API.Config;
using Financer.DataAccess.Services;
using Financer.Infrastructure.Services.JobServices;
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

            var mongoConfig = Configuration.GetSection("Mongo").Get<MongoConfig>();
            services.AddSingleton<IMongoDatabase>(client =>
            {
                var dbClient = new MongoClient(mongoConfig.DbConnectionString);
                return dbClient.GetDatabase(mongoConfig.DatabaseName);
            });
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
