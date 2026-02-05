// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Models.Events;

internal class LegacyTradeEventConsumerJob : BackgroundService
{
    private readonly ILogger<LegacyTradeEventConsumerJob> _logger;

    public LegacyTradeEventConsumerJob(ILogger<LegacyTradeEventConsumerJob> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "test-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        const int CONSUMER_TIMEOUT_SECONDS = 5;
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

        consumer.Subscribe(EventsTopics.LEGACY_PAYMENT_RECIEVED);
        consumer.Subscribe(EventsTopics.LEGACY_REFUND_RECIEVED);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(TimeSpan.FromSeconds(CONSUMER_TIMEOUT_SECONDS));

                if (consumeResult == null)
                    continue;

                _logger.LogInformation($"Offset:{consumeResult.Offset} MSG: {consumeResult.Message.Value}");
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
        consumer.Close();

        return Task.CompletedTask;
    }
}