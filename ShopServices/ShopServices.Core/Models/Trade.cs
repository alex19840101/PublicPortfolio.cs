using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ShopServices.Core.Models
{
    /// <summary> Платеж (транзакция) оплаты/возврата </summary>
    public class Trade
    {
        public long Id { get; private set; }

        /// <summary> Id заказа (при заказе) </summary>
        public uint? OrderId { get; private set; }

        /// <summary> Id покупателя (при заказе) </summary> 
        public uint? BuyerId { get; private set; }

        /// <summary> Cписок оплачиваемых/возвращаемых товарных позиций  </summary>
        public List<OrderPosition> Positions { get; private set; } = default!;

        /// <summary> Оплачиваемая сумма/полная стоимость заказа/чека/квитанции </summary>
        public decimal Amount { get; private set; }

        /// <summary> Валюта </summary>
        public string Currency { get; private set; }

        /// <summary> Дата и время оплаты </summary>
        public DateTime Created { get; private set; }

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; private set; }

        /// <summary> Дополнительная информация </summary>
        public string ExtraInfo { get; private set; }

        /// <summary> Комментарий </summary>
        public string Comment { get; private set; }

        /// <summary> Магазин покупки </summary>
        public uint? ShopId { get; private set; }
        public uint? ManagerId { get; private set; }
        public uint? CourierId { get; private set; }

        /// <summary> Дата и время возврата денежных средств </summary>
        public DateTime? RefundDateTime { get; private set; }

        /// <summary> Сумма возврата стоимости </summary>
        public decimal? RefundAmount { get; private set; }

        /// <summary> Информация по возврату </summary>
        public string RefundInfo { get; private set; }

        /// <summary> В архиве ли (отменен ли) платеж </summary>
        public bool Archived { get; private set; }

        public Trade(
            long id,
            uint? orderId,
            uint? buyerId,
            List<OrderPosition> positions,
            decimal amount,
            string currency,
            DateTime created,
            string paymentInfo,
            string extraInfo,
            string comment,
            uint? shopId,
            uint? managerId,
            uint? courierId,
            DateTime? refundDateTime,
            decimal? refundAmount,
            string refundInfo,
            bool archived)
        {
            Id = id;
            OrderId = orderId;
            BuyerId = buyerId;
            Positions = positions ?? throw new ArgumentNullException(nameof(positions));
            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
            Created = created;
            PaymentInfo = paymentInfo ?? throw new ArgumentNullException(nameof(paymentInfo));
            ExtraInfo = extraInfo;
            Comment = comment;
            ShopId = shopId;
            ManagerId = managerId;
            CourierId = courierId;
            RefundDateTime = refundDateTime;
            RefundAmount = refundAmount;
            RefundInfo = refundInfo;
            Archived = archived;
        }

        public void UpdateBuyer(uint buyerId)
        {
            if (BuyerId != null && BuyerId != buyerId)
                throw new InvalidOperationException($"{ResultMessager.BUYERID_CONFLICT}: buyerId={buyerId} != trade.BuyerId={BuyerId}");

            BuyerId = buyerId;
        }

        /// <summary>
        /// Получение сериализованного списка оплачиваемых/возвращаемых товарных позиций
        /// </summary>
        /// <returns> Cписок оплачиваемых/возвращаемых товарных позиций, сериализованный в строку </returns>
        public string GetPositionsStr() => JsonSerializer.Serialize(Positions);

    }
}
