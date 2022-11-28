using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace KafkaConsumer.Handlers
{
    public class KafkaConsumerHandler : IHostedService
    {
        private ConsumerConfig _config;
        public KafkaConsumerHandler(ConsumerConfig config)
        {
            _config = config;
        }

        private readonly string topic = "simpletalk_topic";
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
                        var consumer = builder.Consume(cancelToken.Token);
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
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