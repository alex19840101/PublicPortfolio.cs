using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Events;
using ShopServices.Core.Services;

namespace Notifications.API.Consumers
{
    /// <summary> MassTransit-Consumer для события "Работник зарегистрировался" </summary>
    public class EmployeeRegisteredEventConsumer : IConsumer<EmployeeRegistered>
    {
        private readonly INotificationsService _notificationsService;
        private readonly IContactsGetterService _contactsGetterService;
        private readonly ILogger<EmployeeRegisteredEventConsumer> _logger;

        /// <summary> MassTransit-Consumer для события "Работник зарегистрировался" </summary>
        /// <param name="notificationsService"></param>
        /// <param name="contactsGetterService"></param>
        /// <param name="logger"></param>
        public EmployeeRegisteredEventConsumer(
            INotificationsService notificationsService,
            IContactsGetterService contactsGetterService,
            ILogger<EmployeeRegisteredEventConsumer> logger)
        {
            _notificationsService = notificationsService;
            _contactsGetterService = contactsGetterService;
            _logger = logger;
        }


        /// <summary> Обработчик события "Работник зарегистрировался" </summary>
        /// <param name="consumeContext"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<EmployeeRegistered> consumeContext)
        {
            var employeeRegistered = consumeContext.Message;
            _logger.LogInformation("> EmployeeRegistered by {EmployeeId}", employeeRegistered.EmployeeId);

            var employeeContactData = await _contactsGetterService.GetEmployeeContactData(employeeRegistered.EmployeeId);

            var notificationMethods = employeeContactData.GetNotificationMethods();

            if (notificationMethods.Contains(NotificationMethod.TelegramMessage))
                await AddNotification(employeeContactData, NotificationMethod.TelegramMessage, employeeRegistered);

            if (notificationMethods.Contains(NotificationMethod.Email))
                await AddNotification(employeeContactData, NotificationMethod.Email, employeeRegistered);

            if (notificationMethods.Contains(NotificationMethod.SMS))
                await AddNotification(employeeContactData, NotificationMethod.SMS, employeeRegistered);

            //local:
            async Task AddNotification(ContactData contactData, NotificationMethod notificationMethod, EmployeeRegistered employeeRegistered)
            {
                await _notificationsService.AddNotification(NotificationsMapper.PrepareCoreNotification(
                            contactData: contactData,
                            notificationMethod: notificationMethod,
                            modelEntityType: ModelEntityType.Employee,
                            changedEntityId: employeeRegistered.EmployeeId,
                            notification: employeeRegistered.Notification,
                            buyerId: null));
            }
        }
    }
}
