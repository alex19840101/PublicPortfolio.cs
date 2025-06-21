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
            string confirmationString,
            string comment);

        /// <summary> Отмена заказа менеджером </summary>
        public Task<Result> CancelOrderByManager(
            uint? managerId,
            uint orderId,
            string confirmationString,
            string comment);

        /// <summary> Подтверждение заказа покупателем </summary>
        public Task<Result> ConfirmOrderByByer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string confirmationString);

        /// <summary> Получение информации о заказе по id заказа </summary>
        public Task<Order> GetOrderInfoById(uint orderId, uint? buyerId);

        /// <summary> Получение информации о заказах покупателя для указанного временного интервала </summary>
        Task<IEnumerable<Order>> GetOrdersByBuyerId(
            uint buyerId,
            uint? buyerIdFromClaim,
            DateTime createdFromDt,
            DateTime? createdToDt,
            uint byPage,
            uint page);

        /// <summary> Отметка заказа как доставленного покупателю </summary>
        public Task<Result> MarkAsDeliveredToBuyer(
            uint orderId,
            string comment,
            uint? courierId);

        /// <summary> Отметка заказа как доставленного (в магазин) </summary>
        public Task<Result> MarkAsDeliveredToShop(
            uint orderId,
            string comment,
            uint? managerId);

        /// <summary> Отметка заказа как полученного покупателем </summary>
        public Task<Result> MarkAsReceived(
            uint orderId,
            string comment,
            uint? managerId,
            uint? courierId);


        public Task<Result> UpdateCourierId(
            uint orderId,
            string comment,
            uint? courierId);

        /// <summary> Изменение адреса доставки заказа (со стороны покупателя) </summary>
        public Task<Result> UpdateDeliveryAddressByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string deliveryAddress);
        public Task<Result> UpdateDeliveryId(
            uint orderId,
            uint? deliveryId,
            uint? managerId,
            uint? courierId,
            string comment);

        /// <summary> Изменение дополнительной информации в заказе (со стороны покупателя) </summary>
        public Task<Result> UpdateExtraInfoByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            string extraInfo);

        /// <summary> Смена менеджера, обслуживающего заказ </summary>
        public Task<Result> UpdateManagerId(
            uint orderId,
            string comment,
            uint managerId);

        /// <summary> Уточнение массы/габаритов заказа (менеджером/курьером) </summary>
        public Task<Result> UpdateMassInGramsDimensions(
            uint orderId,
            uint massInGrams,
            string dimensions,
            string comment,
            uint? managerId,
            uint? courierId);

        /// <summary> Обновление информации об оплате (со стороны менеджера/курьера, обслуживающего заказ) </summary>
        public Task<Result> UpdatePaymentInfo(
            uint orderId,
            string paymentInfo,
            string comment,
            uint? managerId,
            uint? courierId);

        /// <summary> Обновление планируемого срока поставки заказа менеджером, обслуживающим заказ </summary>
        public Task<Result> UpdatePlannedDeliveryTimeByManager(
            uint orderId,
            DateTime plannedDeliveryTime,
            string comment,
            uint managerId);

        /// <summary> Обновление (установка/смена/сброс) магазина выдачи заказа покупателем </summary>
        public Task<Result> UpdateShopIdByBuyer(
            uint buyerIdFromClaim,
            uint buyerIdFromRequest,
            uint orderId,
            uint? shopId);
    }
}
