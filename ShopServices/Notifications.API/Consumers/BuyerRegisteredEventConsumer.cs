using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Events;
using ShopServices.Core.Services;

namespace Notifications.API.Consumers
{
    /// <summary> MassTransit-Consumer для события "Покупатель зарегистрировался" </summary>
    public class BuyerRegisteredEventConsumer : IConsumer<BuyerRegistered>
    {
        private readonly INotificationsService _notificationsService;
        private readonly IContactsGetterService _contactsGetterService;
        private readonly ILogger<BuyerRegisteredEventConsumer> _logger;

        /// <summary> MassTransit-Consumer для события "Покупатель зарегистрировался" </summary>
        /// <param name="notificationsService"></param>
        /// <param name="contactsGetterService"></param>
        /// <param name="logger"></param>
        public BuyerRegisteredEventConsumer(
            INotificationsService notificationsService,
            IContactsGetterService contactsGetterService,
            ILogger<BuyerRegisteredEventConsumer> logger)
        {
            _notificationsService = notificationsService;
            _contactsGetterService = contactsGetterService;
            _logger = logger;
        }


        /// <summary> Обработчик события "Покупатель зарегистрировался" </summary>
        /// <param name="consumeContext"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<BuyerRegistered> consumeContext)
        {
            var buyerRegistered = consumeContext.Message;
            _logger.LogInformation("> BuyerRegistered by {BuyerId}", buyerRegistered.BuyerId);

            var buyerContactData = await _contactsGetterService.GetBuyerContactData(buyerRegistered.BuyerId);

            var notificationMethods = buyerContactData.GetNotificationMethods();

            if (notificationMethods.Contains(NotificationMethod.TelegramMessage))
                await AddNotification(buyerContactData, NotificationMethod.TelegramMessage, buyerRegistered);

            if (notificationMethods.Contains(NotificationMethod.Email))
                await AddNotification(buyerContactData, NotificationMethod.Email, buyerRegistered);

            if (notificationMethods.Contains(NotificationMethod.SMS))
                await AddNotification(buyerContactData, NotificationMethod.SMS, buyerRegistered);

            //local:
            async Task AddNotification(ContactData contactData, NotificationMethod notificationMethod, BuyerRegistered buyerRegistered)
            {
                await _notificationsService.AddNotification(NotificationsMapper.PrepareCoreNotification(
                            contactData: contactData,
                            notificationMethod: notificationMethod,
                            modelEntityType: ModelEntityType.Buyer,
                            changedEntityId: buyerRegistered.BuyerId,
                            notification: buyerRegistered.Notification,
                            buyerId: buyerRegistered.BuyerId));
            }
        }
    }
}
