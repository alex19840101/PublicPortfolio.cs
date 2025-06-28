using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class Delivery
    {
        /// <summary> Уникальный идентификатор доставки заказа </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
        public uint Id { get; private set; }

        /// <summary> Уникальный идентификатор покупателя </summary>
        public uint BuyerId { get; private set; }

        /// <summary> Уникальный идентификатор заказа </summary>
        public uint OrderId { get; private set; }
        public Order Order { get; } = default!;
        public Buyer Buyer { get; } = default!;
        public Manager? Manager { get; private set; }
        public Courier? Courier { get; private set; }

        /// <summary> Код города/населенного пункта </summary>
        [Column(TypeName = "integer")]
        public uint RegionCode { get; private set; } = default!;

        /// <summary> Адрес доставки заказа </summary>
        public string Address { get; private set; } = default!;
        
        /// <summary> Уникальный идентификатор менеджера </summary>
        public uint? ManagerId { get; private set; }
        
        /// <summary> Уникальный идентификатор курьера </summary>
        public uint? CourierId { get; private set; }

        /// <summary> Информация по оплате </summary>
        public string PaymentInfo { get; private set; } = default!;

        /// <summary> Масса, г </summary>
        [Column(TypeName = "integer")]
        public uint MassInGrams { get; private set; } = default!;

        /// <summary> Габариты </summary>
        public string Dimensions { get; private set; } = default!;

        /// <summary> Дата и время создания заказа на доставку </summary>
        public DateTime Created { get; private set; }

        /// <summary> Дата и время обновления данных заказа на доставку </summary>
        public DateTime? Updated { get; private set; }
        [Column(TypeName = "integer")]
        public uint? FromWarehouseId { get; private set; }
        [Column(TypeName = "integer")]
        public uint? ToWarehouseId { get; private set; }
        [Column(TypeName = "integer")]
        public uint? FromShopId { get; private set; }
        [Column(TypeName = "integer")]
        public uint? ToShopId { get; private set; }
        /// <summary> Id трансфера (перевозки заказа/заказов) </summary>
        [Column(TypeName = "integer")]
        public uint? TransferId { get; private set; }

        /// <summary> Комментарий </summary>
        public string Comment { get; private set; } = default!;

        /// <summary> Статус доставки по DeliveryStatus </summary>
        /// <see cref="ShopServices.Core.Enums.DeliveryStatus"/>
        [Column(TypeName = "integer")]
        public uint Status { get; private set; }

        public Delivery(
           uint id,
           uint buyerId,
           uint orderId,
           uint regionCode,
           string address,
           uint? managerId,
           uint? courierId,
           string paymentInfo,
           uint massInGrams,
           string dimensions,
           uint? fromWarehouseId,
           uint? toWarehouseId,
           uint? fromShopId,
           uint? toShopId,
           string comment,
           uint status,
           uint? transferId,
           DateTime created,
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
            Created = created;
            Updated = updated;
            FromWarehouseId = fromWarehouseId;
            ToWarehouseId = toWarehouseId;
            FromShopId = fromShopId;
            ToShopId = toShopId;
            TransferId = transferId;
            Comment = comment;
            Status = status;
        }

        internal void UpdateAddress(string address) => Address = address;
        internal void UpdateComment(string comment) => Comment = comment;
        internal void UpdateCourierId(uint? courierId) => CourierId = courierId;
        internal void UpdateDimensions(string dimensions) => Dimensions = dimensions;
        internal void UpdateFromShopId(uint? fromShopId) => FromShopId = fromShopId;
        internal void UpdateFromWarehouseId(uint? fromWarehouseId) => FromWarehouseId = fromWarehouseId;
        internal void UpdateManagerId(uint managerId) => ManagerId = managerId;
        internal void UpdateMassInGrams(uint massInGrams) => MassInGrams = massInGrams;
        internal void UpdatePaymentInfo(string paymentInfo) => PaymentInfo = paymentInfo;
        internal void UpdateRegionCode(uint regionCode) => RegionCode = regionCode;
        internal void UpdateStatus(uint status) => Status = status;
        internal void UpdateToShopId(uint? toShopId) => ToShopId = toShopId;
        internal void UpdateToWarehouseId(uint? toWarehouseId) => ToWarehouseId = toWarehouseId;
        internal void UpdateTransferId(uint? transferId) => TransferId = transferId;
        internal void UpdateUpdatedDt(DateTime updatedDt) => Updated = updatedDt;
    }
}
