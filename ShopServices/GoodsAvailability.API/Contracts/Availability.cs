using System;

namespace GoodsAvailability.API.Contracts
{
    /// <summary> Информация о наличии (доступности) товара на складе/в магазине </summary>
    public class Availability
    {
        /// <summary> Уникальный идентификатор наличия (доступности) товара на складе/в магазине </summary>
        public ulong Id { get; set; }

        public uint ProductId { get; set; }
        public uint? ShopId { get; set; }
        public uint? WarehouseId { get; set; }
        public int Count { get; set; }

        public uint? ManagerId { get; set; }

        /// <summary> Код города/населенного пункта </summary>
        public uint CityTownCode { get; set; }

        /// <summary> Название магазина/склада </summary>
        public string PlaceName { get; set; }

        /// <summary> Дата и время создания записи </summary>
        public DateTime Created { get; set; }

        /// <summary> Дата и время обновления записи </summary>
        public DateTime? Updated { get; set; }

        /// <summary> Дата и время следующей поставки </summary>
        public DateTime? NextSupplyTime { get; set; }

        /// <summary> Дата и время предыдущей поставки </summary>
        public DateTime? LastSupplyTime { get; set; }
    }
}
