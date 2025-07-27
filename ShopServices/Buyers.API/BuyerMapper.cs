using System;
using System.Linq;
using ShopServices.Core;
using ShopServices.Core.Auth;
using ShopServices.Core.Enums;
using ShopServices.Core.Models;
using ShopServices.Core.Models.Events;

namespace Buyers.API
{
    internal static class BuyerMapper
    {
        internal static Contracts.Buyer GetBuyerDto(Buyer coreBuyer)
        {
            return new Contracts.Buyer
            {
                Id = coreBuyer.Id,
                Login = coreBuyer.Login,
                Name = coreBuyer.Name,
                Surname = coreBuyer.Surname,
                Address = coreBuyer.Address,
                Email = coreBuyer.Email,
                Phones = coreBuyer.Phones,
                Created = coreBuyer.Created,
                Updated = coreBuyer.Updated,
                DiscountGroups = coreBuyer.DiscountGroups
            };
        }

        internal static Buyer GetCoreBuyer(Contracts.Buyer buyerDto)
        {
            return new Buyer(
                id: buyerDto.Id ?? 0,
                login: buyerDto.Login,
                name: buyerDto.Name,
                surname: buyerDto.Surname,
                nick: buyerDto.Nick,
                address: buyerDto.Address,
                email: buyerDto.Email,
                phones: buyerDto.Phones,
                telegramChatId: null,
                notificationMethods: buyerDto.NotificationMethods?.Select(nm => (NotificationMethod)nm).ToList(),
                passwordHash: SHA256Hasher.GeneratePasswordHash(buyerDto.Password, buyerDto.RepeatPassword),
                created: buyerDto.Created,
                updated: buyerDto.Updated,
                discountGroups: buyerDto.DiscountGroups);
        }

        /// <summary> Маппер в класс события "Покупатель зарегистрировался" </summary>
        /// <param name="buyerId"></param>
        /// <returns> Событие "Покупатель зарегистрировался" </returns>
        internal static BuyerRegistered GetBuyerRegistered(uint buyerId)
        {
            return new BuyerRegistered(
                id: Guid.NewGuid(),
                created: DateTime.Now,
                message: nameof(BuyerRegistered),
                notification: NotificationMessages.REGISTERED_NEW_BUYER.Replace("{id}", buyerId.ToString()),
                buyerId: buyerId);
        }

        /// <summary> Маппер в класс события "Покупатель обновил личные данные" </summary>
        /// <param name="buyerId"></param>
        /// <returns> Событие "Покупатель обновил личные данные" </returns>
        internal static BuyerUpdated GetBuyerUpdated(uint buyerId)
        {
            return new BuyerUpdated(
                id: Guid.NewGuid(),
                created: DateTime.Now,
                message: nameof(BuyerUpdated),
                notification: NotificationMessages.BUYER_UPDATED.Replace("{id}", buyerId.ToString()),
                buyerId: buyerId);
        }
    }
}
