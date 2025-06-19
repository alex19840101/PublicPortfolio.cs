using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopServices.Abstractions;
using ShopServices.Core.Models;

namespace ShopServices.Core.Services
{
    public interface IOrdersService
    {
        /// <summary> Добавление заказа </summary>
        public Task<Result> AddOrder(Order order);
        
        /// <summary> Отмена заказа покупателем </summary>
        public Task<Result> CancelOrderByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string confirmationString);

        /// <summary> Отмена заказа менеджером </summary>
        public Task<Result> CancelOrderByManager(
            uint? managerId,
            uint orderId,
            string confirmationString);

        /// <summary> Подтверждение заказа покупателем </summary>
        public Task<Result> ConfirmOrderByByer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string confirmationString);

        /// <summary> Получение информации о заказе по id заказа </summary>
        public Task<Order> GetOrderInfoById(uint orderId, uint? buyerId);

        /// <summary> Изменение адреса доставки заказа (со стороны покупателя) </summary>
        public Task<Result> UpdateDeliveryAddressByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string deliveryAddress);

        /// <summary> Изменение дополнительной информации в заказе (со стороны покупателя) </summary>
        public Task<Result> UpdateExtraInfoByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string extraInfo);

        /// <summary> Обновление (установка/смена/сброс) магазина выдачи заказа покупателем </summary>
        public Task<Result> UpdateShopIdByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            uint? shopId);
    }
}
