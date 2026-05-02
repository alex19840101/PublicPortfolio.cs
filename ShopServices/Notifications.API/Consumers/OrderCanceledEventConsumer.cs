using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Events;
using ShopServices.Core.Services;

namespace Notifications.API.Consumers
{
    /// <summary> MassTransit-Consumer для события "Заказ отменен" </summary>
    public class OrderCanceledEventConsumer : IConsumer<OrderCanceled>
    {
        private readonly INotificationsService _notificationsService;
        private readonly IContactsGetterService _contactsGetterService;
        private readonly ILogger<OrderCanceledEventConsumer> _logger;

        /// <summary> MassTransit-Consumer для события "Заказ отменен" </summary>
        /// <param name="notificationsService"></param>
        /// <param name="contactsGetterService"></param>
        /// <param name="logger"></param>
        public OrderCanceledEventConsumer(
            INotificationsService notificationsService,
            IContactsGetterService contactsGetterService,
            ILogger<OrderCanceledEventConsumer> logger)
        {
            _notificationsService = notificationsService;
            _contactsGetterService = contactsGetterService;
            _logger = logger;
        }


        /// <summary> Обработчик события "Заказ отменен" </summary>
        /// <param name="consumeContext"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<OrderCanceled> consumeContext)
        {
            var orderCanceled = consumeContext.Message;
            _logger.LogInformation("> OrderCanceled {OrderId} by {BuyerId}", orderCanceled.OrderId, orderCanceled.BuyerId);

            var buyerContactData = await _contactsGetterService.GetBuyerContactData(orderCanceled.BuyerId);

            var notificationMethods = buyerContactData.GetNotificationMethods();

            if (notificationMethods.Contains(NotificationMethod.TelegramMessage))
                await AddNotification(buyerContactData, NotificationMethod.TelegramMessage, orderCanceled);

            if (notificationMethods.Contains(NotificationMethod.Email))
                await AddNotification(buyerContactData, NotificationMethod.Email, orderCanceled);

            if (notificationMethods.Contains(NotificationMethod.SMS))
                await AddNotification(buyerContactData, NotificationMethod.SMS, orderCanceled);

            //local:
            async Task AddNotification(ContactData contactData, NotificationMethod notificationMethod, OrderCanceled orderCanceled)
            {
                await _notificationsService.AddNotification(NotificationsMapper.PrepareCoreNotification(
                            contactData: contactData,
                            notificationMethod: notificationMethod,
                            modelEntityType: ModelEntityType.Order,
                            changedEntityId: orderCanceled.OrderId,
                            notification: orderCanceled.Notification,
                            buyerId: orderCanceled.BuyerId));
            }
        }
    }
}
