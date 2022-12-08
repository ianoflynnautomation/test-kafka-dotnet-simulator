using KafkaConsumer.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
namespace KafkaConsumer
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
            services.AddSingleton<IHostedService, KafkaConsumerHandler>();
            services.AddControllers();
            services.AddSingleton<ConsumerConfig>(options =>
            {
                var config = new ConsumerConfig();
                {
                    config.BootstrapServers = Configuration.GetValue<string>("Kafka:ConsumerSettings:BootstrapServers");
                    config.GroupId = Configuration.GetValue<string>("Kafka:ConsumerSettings:GroupId");
                    config.AutoOffsetReset = AutoOffsetReset.Earliest;

                }
                return config;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
