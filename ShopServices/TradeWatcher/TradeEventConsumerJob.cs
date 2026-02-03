// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Models.Events;
using TradeWatcher;

internal class TradeEventConsumerJob : BackgroundService
{
    private ILogger<TradeEventConsumerJob> _logger;

    public TradeEventConsumerJob(ILogger<TradeEventConsumerJob> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:29092",
            GroupId = "test-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        const int CONSUMER_TIMEOUT_SECONDS = 5;

        using var paymentConsumer = new ConsumerBuilder<int, PaymentReceived>(config)
            .SetKeyDeserializer(Deserializers.Int32)
            .SetValueDeserializer(new SystemTextJsonDeserializer<PaymentReceived>())
            .Build();

        paymentConsumer.Subscribe(EventsTopics.PAYMENT_RECIEVED);
        
        using var refundConsumer = new ConsumerBuilder<int, RefundReceived>(config)
            .SetKeyDeserializer(Deserializers.Int32)
            .SetValueDeserializer(new SystemTextJsonDeserializer<RefundReceived>())
            .Build();

        refundConsumer.Subscribe(EventsTopics.REFUND_RECIEVED);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumePaymentResult = paymentConsumer.Consume(TimeSpan.FromSeconds(CONSUMER_TIMEOUT_SECONDS));

                if (consumePaymentResult != null)
                {
                    _logger.LogInformation($"OffSet:{consumePaymentResult.Offset} MSG: {consumePaymentResult.Message.Value}");
                }

                var consumeRefundResult = refundConsumer.Consume(TimeSpan.FromSeconds(CONSUMER_TIMEOUT_SECONDS));

                if (consumeRefundResult != null)
                {
                    _logger.LogInformation($"OffSet:{consumeRefundResult.Offset} MSG: {consumeRefundResult.Message.Value}");
                }
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex.Error.Reason);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        paymentConsumer.Close();
        refundConsumer.Close();
        return Task.CompletedTask;
    }
}