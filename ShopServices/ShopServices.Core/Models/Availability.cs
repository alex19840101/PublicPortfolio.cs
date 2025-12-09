using System;

namespace ShopServices.Core.Models
{
    public class Availability
    {
        /// <summary> Уникальный идентификатор наличия (доступности) товара на складе/в магазине </summary>
        public ulong Id { get; private set; }

        public uint ProductId { get; private set; }
        public uint? ShopId { get; private set; }
        public uint? WarehouseId { get; private set; }
        public int Count { get; private set; }

        public uint? ManagerId { get; private set; }

        /// <summary> Дата и время создания записи </summary>
        public DateTime Created { get; private set; }

        /// <summary> Дата и время обновления записи </summary>
        public DateTime? Updated { get; private set; }

        /// <summary> Дата и время следующей поставки </summary>
        public DateTime? NextSupplyTime { get; private set; }

        /// <summary> Дата и время предыдущей поставки </summary>
        public DateTime? LastSupplyTime { get; private set; }

        public Availability(
            ulong id,
            uint productId,
            uint? shopId,
            uint? warehouseId,
            int count,
            uint? managerId,
            DateTime created,
            DateTime? updated,
            DateTime? nextSupplyTime,
            DateTime? lastSupplyTime
            )
        {
            Id = id;
            ProductId = productId;
            ShopId = shopId;
            WarehouseId = warehouseId;
            Count = count;
            ManagerId = managerId;
            Created = created;
            Updated = updated;
            NextSupplyTime = nextSupplyTime;
            LastSupplyTime = lastSupplyTime;
        }
    }
}
