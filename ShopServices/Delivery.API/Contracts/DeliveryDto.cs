using System;

namespace Deliveries.API.Contracts
{
    /// <summary> Данные перевозки (доставки) заказа </summary>
    public class DeliveryDto
    {
        /// <summary> Уникальный идентификатор доставки заказа </summary>
        public uint Id { get; set; }
        
        /// <summary> Уникальный идентификатор покупателя </summary>
        public uint BuyerId { get; set; }

        /// <summary> Уникальный идентификатор заказа </summary>
        public uint OrderId { get; set; }

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; set; } = default!;

        /// <summary> Уникальный идентификатор менеджера </summary>
        public uint ManagerId { get; set; }

        /// <summary> Уникальный идентификатор курьера </summary>
        public uint? CourierId { get; set; }

        /// <summary> *Код города/населенного пункта </summary>
        public uint RegionCode { get; set; } = default!;

        /// <summary> Адрес доставки заказа </summary>
        public string Address { get; set; } = default!;

        /// <summary> Масса, г </summary>
        public uint MassInGrams { get; set; }

        /// <summary> Габариты </summary>
        public string Dimensions { get; set; } = default!;

        /// <summary> Дата и время создания заказа на доставку </summary>
        public DateTime Created { get; set; }

        /// <summary> Дата и время обновления данных заказа на доставку </summary>
        public DateTime? Updated { get; set; }
        
        /// <summary> Перевозка (доставка) со склада с id {...} </summary>
        public uint? FromWarehouseId { get; set; }

        /// <summary> Перевозка (доставка) на склад с id {...} </summary>
        public uint? ToWarehouseId { get; set; }

        /// <summary> Перевозка (доставка) из магазина с id {...} </summary>
        public uint? FromShopId { get; set; }

        /// <summary> Перевозка (доставка) в магазин с id {...} </summary>
        public uint? ToShopId { get; set; }

        /// <summary> Id трансфера (перевозки заказа/заказов) </summary>
        public uint? TransferId { get; set; }

        /// <summary> Комментарий </summary>
        public string Comment { get; set; } = default!;
        /// <summary> Статус </summary>
        public uint Status { get; set; }
    }
}
