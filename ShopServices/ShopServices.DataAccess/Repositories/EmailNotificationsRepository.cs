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
    public class EmailNotificationsRepository : IEmailNotificationsRepository
    {
        private readonly ShopServicesDbContext _dbContext;

        public EmailNotificationsRepository(ShopServicesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Create(Notification newNotification)
        {
            var newEmailNotificationEntity = new Entities.EmailNotification(
                id: 0,
                modelEntityType: (uint)newNotification.ModelEntityType,
                buyerId: newNotification.BuyerId,
                changedEntityId: newNotification.ChangedEntityId,
                emailFrom: newNotification.Sender,
                emailTo: newNotification.Recipient,
                topic: newNotification.Topic,
                message: newNotification.Message,
                created: DateTime.Now.ToUniversalTime(),
                creator: newNotification.Creator);

            await _dbContext.EmailNotifications.AddAsync(newEmailNotificationEntity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(newEmailNotificationEntity).GetDatabaseValuesAsync(); //получение сгенерированного БД id
            return new Result
            {
                Id = newEmailNotificationEntity.Id,
                StatusCode = HttpStatusCode.Created,
                Message = ResultMessager.OK
            };
        }

        public async Task<Notification?> GetEmailNotificationDataById(ulong notificationId)
        {
            var emailNotificationEntity = await _dbContext.EmailNotifications
                .AsNoTracking()
                .Where(en => en.Id == notificationId).SingleOrDefaultAsync();

            if (emailNotificationEntity is null)
                return null;

            return GetCoreNotification(emailNotificationEntity);
        }

        public async Task<IEnumerable<Notification>> GetEmailNotificationsData(
            uint buyerId,
            uint? orderId,
            uint take,
            uint skipCount)
        {
            var limitCount = take > 100 ? 100 : take;

            List<Entities.EmailNotification> entityNotificationsLst = orderId == null ?
                await _dbContext.EmailNotifications.AsNoTracking().Where(emailNotification => emailNotification.BuyerId == buyerId)
                    .Skip((int)skipCount).Take((int)limitCount).ToListAsync() :
                await _dbContext.EmailNotifications.AsNoTracking().Where(en => en.ChangedEntityId == orderId && en.ModelEntityType == (uint)ModelEntityType.Order && en.BuyerId == buyerId)
                    .Skip((int)skipCount).Take((int)limitCount).ToListAsync();

            return entityNotificationsLst.Select(emailNotification => GetCoreNotification(emailNotification));
        }

        public async Task<Result> UpdateSent(ulong notificationId, DateTime sent)
        {
            var emailNotificationEntity = await _dbContext.EmailNotifications
                .Where(en => en.Id == notificationId).SingleOrDefaultAsync();

            if (emailNotificationEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (emailNotificationEntity.Sent == null)
                emailNotificationEntity.UpdateSent(sent.ToUniversalTime());

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.EMAIL_NOTIFICATION_SENT_EARLIER, HttpStatusCode.OK);
        }

        public async Task<Result> SaveUnsuccessfulAttempt(ulong notificationId, DateTime lastUnsuccessfulAttempt)
        {
            var emailNotificationEntity = await _dbContext.EmailNotifications
                .Where(en => en.Id == notificationId).SingleOrDefaultAsync();

            if (emailNotificationEntity is null)
                return new Result(ResultMessager.NOT_FOUND, HttpStatusCode.NotFound);

            if (emailNotificationEntity.LastUnsuccessfulAttempt?.ToLocalTime() != lastUnsuccessfulAttempt)
                emailNotificationEntity.UnsuccessfulAttempt(lastUnsuccessfulAttempt.ToUniversalTime());

            if (_dbContext.ChangeTracker.HasChanges())
            {
                await _dbContext.SaveChangesAsync();
                return new Result(ResultMessager.OK, HttpStatusCode.OK);
            }

            return new Result(ResultMessager.UNSUCCESSFULL_ATTEMPT_SAVED_EARLIER, HttpStatusCode.OK);
        }

        /// <summary>
        /// Маппер Entities.EmailNotification - ShopServices.Core.Models.Notification
        /// </summary>
        /// <param name="emailNotificationEntity"></param>
        /// <returns></returns>
        private static Notification GetCoreNotification(Entities.EmailNotification emailNotificationEntity)
        {
            return new Notification(
                id: emailNotificationEntity.Id,
                notificationMethod: NotificationMethod.Email,
                modelEntityType: (ModelEntityType)emailNotificationEntity.ModelEntityType,
                buyerId: emailNotificationEntity.BuyerId,
                changedEntityId: emailNotificationEntity.ChangedEntityId,
                sender: emailNotificationEntity.EmailFrom,
                recipient: emailNotificationEntity.EmailTo,
                topic: emailNotificationEntity.Topic,
                message: emailNotificationEntity.Message,
                created: emailNotificationEntity.Created,
                creator: emailNotificationEntity.Creator,
                sent: emailNotificationEntity.Sent,
                unsuccessfulAttempts: emailNotificationEntity.UnsuccessfulAttempts,
                lastUnsuccessfulAttempt: emailNotificationEntity.LastUnsuccessfulAttempt);
        }
    }
}
