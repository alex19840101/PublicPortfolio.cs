using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Trade.API
{
    /// <summary> Kafka-продюсер </summary>
    public class ProducerService
    {
        private readonly ILogger<ProducerService> _logger;
        private readonly ProducerConfig _producerConfig;
        
        /// <summary> Конструктор Kafka-продюсера </summary>
        public ProducerService(ILogger<ProducerService> logger)
        {
            _logger = logger;

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:29092",
                AllowAutoCreateTopics = true,
                Acks = Acks.All
            };
        }

        /// <summary> Отправка в Kafka </summary>
        public async Task ProduceAsync(string topic, string message, CancellationToken cancellationToken)
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();

            try
            {
                var deliveryResult = await producer.ProduceAsync(
                    topic,
                    message: new Message<Null, string> { Value = message },
                    cancellationToken);

                _logger.LogTrace($"отправлено в Kafka: {deliveryResult.Value}, Offset: {deliveryResult.Offset}");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Ошибка отправки в Kafka: {e.Error.Reason}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Необрабатываемая ошибка отправки в Kafka: {e}");
            }

            producer.Flush(cancellationToken);
        }

    }
}
