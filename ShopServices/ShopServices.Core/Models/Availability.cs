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

        /// <summary> Название магазина/склада </summary>
        public string PlaceName { get; private set; }

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
            string placeName,
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
            PlaceName = placeName;
            Created = created;
            Updated = updated;
            NextSupplyTime = nextSupplyTime;
            LastSupplyTime = lastSupplyTime;
        }


        /// <summary>
        /// Проверка на равенство (существующей Availability) с игнорированием:
        /// <para> - Id (Id до момента добавления не определен, как бы равен нулю),</para>
        /// <para> - Created </para>
        /// </summary>
        /// <param name="comparedAvailability"> Availability для сравнения</param>
        /// <returns></returns>
        public bool IsEqualIgnoreIdAndDt(Availability comparedAvailability)
        {
            if (comparedAvailability.ProductId != ProductId ||
                !string.Equals(comparedAvailability.PlaceName, PlaceName) ||
                comparedAvailability.Count != Count ||
                comparedAvailability.ShopId != ShopId ||
                comparedAvailability.ShopId != ShopId ||
                comparedAvailability.WarehouseId != WarehouseId ||
                comparedAvailability.ManagerId != ManagerId ||
                comparedAvailability.Updated != Updated ||
                comparedAvailability.NextSupplyTime != NextSupplyTime ||
                comparedAvailability.LastSupplyTime != LastSupplyTime)
                return false;

            return true;
        }
    }
}
