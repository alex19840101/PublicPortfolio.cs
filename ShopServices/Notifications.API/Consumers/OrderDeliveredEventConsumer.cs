using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Events;
using ShopServices.Core.Services;

namespace Notifications.API.Consumers
{
    /// <summary> MassTransit-Consumer для события "Заказ доставлен" </summary>
    public class OrderDeliveredEventConsumer : IConsumer<OrderDelivered>
    {
        private readonly INotificationsService _notificationsService;
        private readonly IContactsGetterService _contactsGetterService;
        private readonly ILogger<OrderDeliveredEventConsumer> _logger;

        /// <summary> MassTransit-Consumer для события "Заказ доставлен" </summary>
        /// <param name="notificationsService"></param>
        /// <param name="contactsGetterService"></param>
        /// <param name="logger"></param>
        public OrderDeliveredEventConsumer(
            INotificationsService notificationsService,
            IContactsGetterService contactsGetterService,
            ILogger<OrderDeliveredEventConsumer> logger)
        {
            _notificationsService = notificationsService;
            _contactsGetterService = contactsGetterService;
            _logger = logger;
        }


        /// <summary> Обработчик события "Заказ доставлен" </summary>
        /// <param name="consumeContext"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<OrderDelivered> consumeContext)
        {
            var orderDelivered = consumeContext.Message;
            _logger.LogInformation("> OrderDelivered {OrderId} by {BuyerId}", orderDelivered.OrderId, orderDelivered.BuyerId);

            var buyerContactData = await _contactsGetterService.GetBuyerContactData(orderDelivered.BuyerId);

            var notificationMethods = buyerContactData.GetNotificationMethods();

            if (notificationMethods.Contains(NotificationMethod.TelegramMessage))
                await AddNotification(buyerContactData, NotificationMethod.TelegramMessage, orderDelivered);

            if (notificationMethods.Contains(NotificationMethod.Email))
                await AddNotification(buyerContactData, NotificationMethod.Email, orderDelivered);

            if (notificationMethods.Contains(NotificationMethod.SMS))
                await AddNotification(buyerContactData, NotificationMethod.SMS, orderDelivered);

            //local:
            async Task AddNotification(ContactData contactData, NotificationMethod notificationMethod, OrderDelivered orderDelivered)
            {
                await _notificationsService.AddNotification(NotificationsMapper.PrepareCoreNotification(
                            contactData: contactData,
                            notificationMethod: notificationMethod,
                            modelEntityType: ModelEntityType.Order,
                            changedEntityId: orderDelivered.OrderId,
                            notification: orderDelivered.Notification,
                            buyerId: orderDelivered.BuyerId));
            }
        }
    }
}
