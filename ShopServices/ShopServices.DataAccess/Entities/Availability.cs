using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    public class Availability
    {
        /// <summary> Уникальный идентификатор наличия (доступности) товара на складе/в магазине </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "bigint")]
        public ulong Id { get; private set; }

        public uint ProductId { get; private set; }
        [Column(TypeName = "integer")]
        public uint? ShopId { get; private set; }
        [Column(TypeName = "integer")]
        public uint? WarehouseId { get; private set; }
        public int Count { get; private set; }
    }
}
