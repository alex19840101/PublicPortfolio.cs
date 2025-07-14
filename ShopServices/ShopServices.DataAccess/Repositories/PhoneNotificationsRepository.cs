using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopServices.Abstractions;
using ShopServices.Core;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Repositories;

namespace ShopServices.DataAccess.Repositories
{
    public class PhoneNotificationsRepository : IPhoneNotificationsRepository
    {
        private readonly ShopServicesDbContext _dbContext;
        private const int MAX_RETRY_COUNT = 10;
        public PhoneNotificationsRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Create(Notification newNotification)
        {
            ArgumentNullException.ThrowIfNull(newNotification);

            var newPhoneNotificationEntity = new Entities.PhoneNotification(
                id: 0,
                notificationMethod: (uint)newNotification.NotificationMethod,
                modelEntityType: (uint)newNotification.ModelEntityType,
                buyerId: newNotification.BuyerId,
                changedEntityId: newNotification.ChangedEntityId,
                smsFrom: newNotification.Sender,
                smsTo: newNotification.Recipient,
                message: newNotification.Message,
                created: DateTime.Now.ToUniversalTime(),
                creator: newNotification.Creator);

            await _dbContext.PhoneNotifications.AddAsync(newPhoneNotificationEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newPhoneNotificationEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new Result
            {
                Id = newPhoneNotificationEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Notification?> GetPhoneNotificationDataById(ulong notificationId)
        {
            var phoneNotificationEntity = await _dbContext.PhoneNotifications
                .AsNoTracking()
                .Where(pn => pn.Id == notificationId).SingleOrDefaultAsync();

            if (phoneNotificationEntity is null)
                return null;

            return GetCoreNotification(phoneNotificationEntity);
        }

        public async Task<IEnumerable<Notification>> GetPhoneNotificationsData(
            uint buyerId,
            uint? orderId,
            uint take,
            uint skipCount)
        {
            var limitCount = take > 100 ? 100 : take;

            List<Entities.PhoneNotification> phoneNotificationsLst = orderId == null ?
                await _dbContext.PhoneNotifications.AsNoTracking().Where(phoneNotification => phoneNotification.BuyerId == buyerId)
                .Skip((int)skipCount).Take((int)limitCount).ToListAsync() :
                await _dbContext.PhoneNotifications.AsNoTracking().Where(pn => pn.ChangedEntityId == orderId && pn.ModelEntityType == (uint)ModelEntityType.Order && pn.BuyerId == buyerId)
                .Skip((int)skipCount).Take((int)limitCount).ToListAsync();

            return phoneNotificationsLst.Select(phoneNotification => GetCoreNotification(phoneNotification));
        }

        public async Task<IEnumerable<Notification>> GetPhoneNotificationsToSend(
            ulong minNotificationId,
            uint take = 1000)
        {
            var limitCount = take > 1000 ? 1000 : take;
            List<Entities.PhoneNotification> phoneNotificationsLst = await _dbContext.PhoneNotifications.AsNoTracking()
                .Where(phoneNotification => phoneNotification.Id >= minNotificationId && phoneNotification.UnsuccessfulAttempts < MAX_RETRY_COUNT)
                .OrderBy(phoneNotification => phoneNotification.Id)
                .Take((int)limitCount).ToListAsync();

            return phoneNotificationsLst.Select(phoneNotification => GetCoreNotification(phoneNotification));
        }

        public async Task<Result> UpdateSent(ulong notificationId, DateTime sent)
        {
            var phoneNotificationEntity = await _dbContext.PhoneNotifications
                .Where(en => en.Id == notificationId).SingleOrDefaultAsync();

            if (phoneNotificationEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (phoneNotificationEntity.Sent == null)
                phoneNotificationEntity.UpdateSent(sent.ToUniversalTime());

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.PHONE_NOTIFICATION_SENT_EARLIER, HttpStatusCode.OK);
        }

        public async Task<Result> SaveUnsuccessfulAttempt(ulong notificationId, DateTime lastUnsuccessfulAttempt)
        {
            var phoneNotificationEntity = await _dbContext.PhoneNotifications
                .Where(en => en.Id == notificationId).SingleOrDefaultAsync();

            if (phoneNotificationEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (phoneNotificationEntity.LastUnsuccessfulAttempt?.ToLocalTime() != lastUnsuccessfulAttempt)
                phoneNotificationEntity.UnsuccessfulAttempt(lastUnsuccessfulAttempt.ToUniversalTime());

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.UNSUCCESSFULL_ATTEMPT_SAVED_EARLIER, HttpStatusCode.OK);
        }

        /// <summary>
        /// Маппер Entities.PhoneNotification - ShopServices.Core.Models.Notification
        /// </summary>
        /// <param name="phoneNotificationEntity"></param>
        /// <returns></returns>
        private static Notification GetCoreNotification(Entities.PhoneNotification phoneNotificationEntity)
        {
            return new Notification(
                id: phoneNotificationEntity.Id,
                notificationMethod: (NotificationMethod)phoneNotificationEntity.NotificationMethod,
                modelEntityType: (ModelEntityType)phoneNotificationEntity.ModelEntityType,
                buyerId: phoneNotificationEntity.BuyerId,
                changedEntityId: phoneNotificationEntity.ChangedEntityId,
                sender: phoneNotificationEntity.SmsFrom,
                recipient: phoneNotificationEntity.SmsTo,
                topic: string.Empty,
                message: phoneNotificationEntity.Message,
                created: phoneNotificationEntity.Created,
                creator: phoneNotificationEntity.Creator,
                sent: phoneNotificationEntity.Sent,
                unsuccessfulAttempts: phoneNotificationEntity.UnsuccessfulAttempts,
                lastUnsuccessfulAttempt: phoneNotificationEntity.LastUnsuccessfulAttempt);
        }
    }
}
