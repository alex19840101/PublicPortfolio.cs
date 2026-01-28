using System;
using System.Collections.Generic;

namespace Trade.API.Contracts.Responses
{
    /// <summary> Информация о транзакции оплаты/возврата </summary>
    public class TransactionInfoResponseDto
    {
        /// <summary> Id транзакции оплаты/возврата совершенной ранее покупки (заказа/чека/квитанции) </summary>
        public long Id { get; set; }

        /// <summary> Id заказа (при заказе) </summary>
        public uint? OrderId { get; set; }

        /// <summary> Id покупателя (при заказе) </summary> 
        public uint? BuyerId { get; set; }

        /// <summary> Список товарных позиций в заказе/чеке/квитанции </summary>
        public List<OrderPositionResponseDto> Positions { get; set; } = default!;

        /// <summary> Полная стоимость заказа/чека/квитанции </summary>
        public decimal Cost { get; set; }

        /// <summary> Валюта </summary>
        public string Currency { get; set; } = default!;

        /// <summary> Дата и время оплаты </summary>
        public DateTime Created { get; set; }

        /// <summary> Информация по транзакции возврата </summary>
        public string PaymentInfo { get; set; } = default!;

        /// <summary> Дополнительная информация по транзакции возврата </summary>
        public string? ExtraInfo { get; set; }

        /// <summary> Комментарий </summary>
        public string? Comment { get; set; }

        /// <summary> Магазин возврата </summary>
        public uint? ShopId { get; set; }

        /// <summary> Id менеджера (при возврате менеджером) </summary>
        public uint? ManagerId { get; set; }

        /// <summary> Id курьера (при возврате курьером) </summary>
        public uint? CourierId { get; set; }

        /// <summary> Дата и время возврата денежных средств </summary>
        public DateTime? RefundDateTime { get; set; }

        /// <summary> Сумма возврата стоимости </summary>
        public decimal? RefundAmount { get; set; }

        /// <summary> Информация по возврату </summary>
        public string RefundInfo { get; set; } = default!;

        /// <summary> В архиве ли (отменен ли) платеж </summary>
        public bool Archived { get; set; }
    }
}
