using System;
using System.Collections.Generic;
using System.Text;
using ShopServices.Core.Repositories;
using ShopServices.Core.Services;

namespace ShopServices.BusinessLogic
{
    public class NotificationsService : INotificationsService
    {
        private readonly IEmailNotificationsRepository _emailNotificationsRepository;
        private readonly IPhoneNotificationsRepository _phoneNotificationsRepository;

        public NotificationsService(
            IEmailNotificationsRepository emailNotificationsRepository,
            IPhoneNotificationsRepository phoneNotificationsRepository)
        {
            _emailNotificationsRepository = emailNotificationsRepository;
            _phoneNotificationsRepository = phoneNotificationsRepository;
        }
    }
}
