using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Events;
using ShopServices.Core.Services;

namespace Notifications.API.Consumers
{
    /// <summary> MassTransit-Consumer для события "Работник обновил личные данные" </summary>
    public class EmployeeUpdatedEventConsumer : IConsumer<EmployeeUpdated>
    {
        private readonly INotificationsService _notificationsService;
        private readonly IContactsGetterService _contactsGetterService;
        private readonly ILogger<EmployeeUpdatedEventConsumer> _logger;

        /// <summary> MassTransit-Consumer для события "Работник обновил личные данные" </summary>
        /// <param name="notificationsService"></param>
        /// <param name="contactsGetterService"></param>
        /// <param name="logger"></param>
        public EmployeeUpdatedEventConsumer(
            INotificationsService notificationsService,
            IContactsGetterService contactsGetterService,
            ILogger<EmployeeUpdatedEventConsumer> logger)
        {
            _notificationsService = notificationsService;
            _contactsGetterService = contactsGetterService;
            _logger = logger;
        }


        /// <summary> Обработчик события "Работник обновил личные данные" </summary>
        /// <param name="consumeContext"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<EmployeeUpdated> consumeContext)
        {
            var employeeUpdated = consumeContext.Message;
            _logger.LogInformation("> EmployeeUpdated by {EmployeeId}", employeeUpdated.EmployeeId);

            var employeeContactData = await _contactsGetterService.GetEmployeeContactData(employeeUpdated.EmployeeId);

            var notificationMethods = employeeContactData.GetNotificationMethods();

            if (notificationMethods.Contains(NotificationMethod.TelegramMessage))
                await AddNotification(employeeContactData, NotificationMethod.TelegramMessage, employeeUpdated);

            if (notificationMethods.Contains(NotificationMethod.Email))
                await AddNotification(employeeContactData, NotificationMethod.Email, employeeUpdated);

            if (notificationMethods.Contains(NotificationMethod.SMS))
                await AddNotification(employeeContactData, NotificationMethod.SMS, employeeUpdated);

            //local:
            async Task AddNotification(ContactData contactData, NotificationMethod notificationMethod, EmployeeUpdated employeeUpdated)
            {
                await _notificationsService.AddNotification(NotificationsMapper.PrepareCoreNotification(
                            contactData: contactData,
                            notificationMethod: notificationMethod,
                            modelEntityType: ModelEntityType.Employee,
                            changedEntityId: employeeUpdated.EmployeeId,
                            notification: employeeUpdated.Notification,
                            buyerId: employeeUpdated.EmployeeId));
            }
        }
    }
}
