using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
