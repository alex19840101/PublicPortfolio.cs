using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Models.Events;
using ShopServices.Core.Services;

namespace Notifications.API.Consumers
{
    /// <summary>
    /// MassTransit-Consumer для события "Заказ создан"
    /// </summary>
    public class OrderCreatedEventConsumer : IConsumer<OrderCreated>
    {
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;

        public OrderCreatedEventConsumer(
            INotificationsService notificationsService,
            ILogger<OrderCreatedEventConsumer> logger)
        {
            _notificationsService = notificationsService;
            _logger = logger;
        }


        /// <summary> Обработчик события "Заказ создан" </summary>
        /// <param name="consumeContext"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<OrderCreated> consumeContext)
        {
            var orderCreated = consumeContext.Message;
            _logger.LogInformation("> OrderCreated {OrderId} by {BuyerId}", orderCreated.OrderId, orderCreated.BuyerId);
        }
    }
}
