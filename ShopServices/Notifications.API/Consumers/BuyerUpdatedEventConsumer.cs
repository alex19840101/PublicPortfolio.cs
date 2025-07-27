using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Events;
using ShopServices.Core.Services;

namespace Notifications.API.Consumers
{
    /// <summary> MassTransit-Consumer для события "Покупатель обновил личные данные" </summary>
    public class BuyerUpdatedEventConsumer : IConsumer<BuyerUpdated>
    {
        private readonly INotificationsService _notificationsService;
        private readonly IContactsGetterService _contactsGetterService;
        private readonly ILogger<BuyerUpdatedEventConsumer> _logger;

        /// <summary> MassTransit-Consumer для события "Покупатель обновил личные данные" </summary>
        /// <param name="notificationsService"></param>
        /// <param name="contactsGetterService"></param>
        /// <param name="logger"></param>
        public BuyerUpdatedEventConsumer(
            INotificationsService notificationsService,
            IContactsGetterService contactsGetterService,
            ILogger<BuyerUpdatedEventConsumer> logger)
        {
            _notificationsService = notificationsService;
            _contactsGetterService = contactsGetterService;
            _logger = logger;
        }


        /// <summary> Обработчик события "Покупатель обновил личные данные" </summary>
        /// <param name="consumeContext"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<BuyerUpdated> consumeContext)
        {
            var buyerUpdated = consumeContext.Message;
            _logger.LogInformation("> BuyerUpdated by {BuyerId}", buyerUpdated.BuyerId);

            var buyerContactData = await _contactsGetterService.GetBuyerContactData(buyerUpdated.BuyerId);

            var notificationMethods = buyerContactData.GetNotificationMethods();

            if (notificationMethods.Contains(NotificationMethod.TelegramMessage))
                await AddNotification(buyerContactData, NotificationMethod.TelegramMessage, buyerUpdated);

            if (notificationMethods.Contains(NotificationMethod.Email))
                await AddNotification(buyerContactData, NotificationMethod.Email, buyerUpdated);

            if (notificationMethods.Contains(NotificationMethod.SMS))
                await AddNotification(buyerContactData, NotificationMethod.SMS, buyerUpdated);

            //local:
            async Task AddNotification(ContactData contactData, NotificationMethod notificationMethod, BuyerUpdated buyerUpdated)
            {
                await _notificationsService.AddNotification(NotificationsMapper.PrepareCoreNotification(
                            contactData: contactData,
                            notificationMethod: notificationMethod,
                            modelEntityType: ModelEntityType.Buyer,
                            changedEntityId: buyerUpdated.BuyerId,
                            notification: buyerUpdated.Notification,
                            buyerId: buyerUpdated.BuyerId));
            }
        }
    }
}
