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
                BootstrapServers = "localhost:29092,localhost:39092,localhost:49092",
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
                    topic: topic,
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

        /// <summary> Отправка в Kafka </summary>
        /// <param name="topic"> Тopic - топик - тип (тема) сообщения </param>
        /// <param name="message"> (TKey, TValue) - TKey - ключ, равный Id отслеживаемой сущности (id покупателя/заказа), TValue - тип сообщения для использования в Kafka </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        public async Task ProduceAsync<TKey, TValue>(string topic, Message<TKey, TValue> message, CancellationToken cancellationToken)
        {
            using var producer = new ProducerBuilder<TKey, TValue>(_producerConfig)
                .SetValueSerializer(new SystemTextJSonSerializer<TValue>())
                .Build();

            try
            {
                var deliveryResult = await producer.ProduceAsync(
                    topic: topic,
                    message: message,
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
