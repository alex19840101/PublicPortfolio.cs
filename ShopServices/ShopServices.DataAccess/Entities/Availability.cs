using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class Availability
    {
        /// <summary> Уникальный идентификатор наличия (доступности) товара на складе/в магазине </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; private set; }

        public uint ProductId { get; private set; }
        public uint? ShopId { get; private set; }
        public uint? WarehouseId { get; private set; }
        public int Count { get; private set; }
    }
}
