using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    /// <summary> Информация о наличии (доступности) товара на складе/в магазине </summary>
    public class Availability
    {
        /// <summary> Уникальный идентификатор наличия (доступности) товара на складе/в магазине </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "bigint")]
        public ulong Id { get; private set; }

        public uint ProductId { get; private set; }
        
        [Column(TypeName = "integer")]
        public uint? ShopId { get; private set; }
        /// <summary> Навигационное свойство - магазин </summary>
        public Shop Shop { get; }
        
        [Column(TypeName = "integer")]
        public uint? WarehouseId { get; private set; }

        /// <summary> Навигационное свойство - склад </summary>
        public Warehouse? Warehouse { get; }
        
        [Column(TypeName = "integer")]
        public int Count { get; private set; }

        [Column(TypeName = "integer")]
        public uint? ManagerId { get; private set; }

        /// <summary> Навигационное свойство - менеджер </summary>
        public Manager? Manager { get; private set; }
        
        [Column(TypeName = "integer")]
        public uint CityTownCode { get; private set; }

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
            uint cityTownCode,
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
            CityTownCode = cityTownCode;
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

        public void UpdatePlaceName(string placeName) => PlaceName = placeName;
        public void UpdateUpdated(DateTime? updated) => Updated = updated;
        public void UpdateNextSupplyTime(DateTime? nextSupplyTime) => NextSupplyTime = nextSupplyTime;
        public void UpdateLastSupplyTime(DateTime? lastSupplyTime) => LastSupplyTime = lastSupplyTime;
        public void UpdateCount(int count) => Count = count;
        public void UpdateManagerId(uint? managerId) => ManagerId = managerId;
    }
}
