using System;
using ShopServices.Core.Enums;

namespace ShopServices.Core.Models
{
    public class Delivery
    {
        /// <summary> Уникальный идентификатор доставки заказа </summary>
        public uint Id { get; private set; }

        /// <summary> Уникальный идентификатор покупателя </summary>
        public uint BuyerId { get; }
        
        /// <summary> Уникальный идентификатор заказа </summary>
        public uint OrderId { get; }

        /// <summary> Код города/населенного пункта </summary>
        public uint RegionCode { get; private set; }

        /// <summary> Адрес доставки заказа </summary>
        public string Address { get; private set; } = default!;
        
        /// <summary> Уникальный идентификатор менеджера </summary>
        public uint ManagerId { get; private set; }
        
        /// <summary> Уникальный идентификатор курьера </summary>
        public uint? CourierId { get; private set; }

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; private set; } = default!;

        /// <summary> Масса, г </summary>
        public uint MassInGrams { get; private set; }

        /// <summary> Габариты </summary>
        public string Dimensions { get; private set; } = default!;

        /// <summary> Дата и время создания заказа на доставку </summary>
        public DateTime? CreatedDt { get; private set; }

        /// <summary> Дата и время обновления данных заказа на доставку </summary>
        public DateTime? Updated { get; private set; }

        public uint? FromWarehouseId { get; private set; }
        public uint? ToWarehouseId { get; private set; }
        public uint? FromShopId { get; private set; }
        public uint? ToShopId { get; private set; }
        /// <summary> Id трансфера (перевозки заказа/заказов) </summary>
        public uint? TransferId { get; private set; }

        /// <summary> Комментарий </summary>
        public string Comment { get; private set; } = default!;
        /// <summary> Статус доставки по DeliveryStatus </summary>
        /// <see cref="DeliveryStatus"/>
        public DeliveryStatus Status { get; private set; }

        public Delivery(
           uint id,
           uint buyerId,
           uint orderId,
           uint regionCode,
           string address,
           uint managerId,
           uint? courierId,
           string paymentInfo,
           uint massInGrams,
           string dimensions,
           uint? fromWarehouseId,
           uint? toWarehouseId,
           uint? fromShopId,
           uint? toShopId,
           string comment,
           DeliveryStatus status,
           uint? transferId,
           DateTime? createdDt = null,
           DateTime? updated = null)
        {
            Id = id;
            BuyerId = buyerId;
            OrderId = orderId;
            RegionCode = regionCode;
            Address = address;
            ManagerId = managerId;
            CourierId = courierId;
            PaymentInfo = paymentInfo;
            MassInGrams = massInGrams;
            Dimensions = dimensions;
            CreatedDt = createdDt;
            Updated = updated;
            FromWarehouseId = fromWarehouseId;
            ToWarehouseId = toWarehouseId;
            FromShopId = fromShopId;
            ToShopId = toShopId;
            TransferId = transferId;
            Comment = comment;
            Status = status;
        }
    }
}
