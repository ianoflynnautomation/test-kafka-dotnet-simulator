using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
namespace KafkaConsumer.Handlers
{
    public class KafkaConsumerHandler : IHostedService
    {
        private ConsumerConfig _config;
        public KafkaConsumerHandler(ConsumerConfig config)
        {
            _config = config;
        }

        private readonly string topic = "test_topic";
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var builder = new ConsumerBuilder<Ignore, string>(_config).Build())
            {
                builder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var consumed = builder.Consume(cancelToken.Token);
                        var orderRequest = JsonSerializer.Deserialize<ProcessOrder>(consumed.Message.Value);
                        Debug.WriteLine($"Processing Order Id:{orderRequest.OrderId}");
                    }
                }
                catch (Exception)
                {
                    builder.Close();
                }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}