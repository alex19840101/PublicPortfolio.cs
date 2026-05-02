using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Events;
using ShopServices.Core.Services;

namespace Notifications.API.Consumers
{
    /// <summary> MassTransit-Consumer для события "Заказ создан" </summary>
    public class OrderCreatedEventConsumer : IConsumer<OrderCreated>
    {
        private readonly INotificationsService _notificationsService;
        private readonly IContactsGetterService _contactsGetterService;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;

        /// <summary> MassTransit-Consumer для события "Заказ создан" </summary>
        /// <param name="notificationsService"></param>
        /// <param name="contactsGetterService"></param>
        /// <param name="logger"></param>
        public OrderCreatedEventConsumer(
            INotificationsService notificationsService,
            IContactsGetterService contactsGetterService,
            ILogger<OrderCreatedEventConsumer> logger)
        {
            _notificationsService = notificationsService;
            _contactsGetterService = contactsGetterService;
            _logger = logger;
        }


        /// <summary> Обработчик события "Заказ создан" </summary>
        /// <param name="consumeContext"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<OrderCreated> consumeContext)
        {
            var orderCreated = consumeContext.Message;
            _logger.LogInformation("> OrderCreated {OrderId} by {BuyerId}", orderCreated.OrderId, orderCreated.BuyerId);

            var buyerContactData = await _contactsGetterService.GetBuyerContactData(orderCreated.BuyerId);

            var notificationMethods = buyerContactData.GetNotificationMethods();

            if (notificationMethods.Contains(NotificationMethod.TelegramMessage))
                await AddNotification(buyerContactData, NotificationMethod.TelegramMessage, orderCreated);

            if (notificationMethods.Contains(NotificationMethod.Email))
                await AddNotification(buyerContactData, NotificationMethod.Email, orderCreated);

            if (notificationMethods.Contains(NotificationMethod.SMS))
                await AddNotification(buyerContactData, NotificationMethod.SMS, orderCreated);
            
            //local:
            async Task AddNotification(ContactData contactData, NotificationMethod notificationMethod, OrderCreated orderCreated)
            {
                await _notificationsService.AddNotification(NotificationsMapper.PrepareCoreNotification(
                            contactData: contactData,
                            notificationMethod: notificationMethod,
                            modelEntityType: ModelEntityType.Order,
                            changedEntityId: orderCreated.OrderId,
                            notification: orderCreated.Notification,
                            buyerId: orderCreated.BuyerId));
            }
        }
    }
}
