using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    /// <summary> Товарная позиция в заказе </summary>
    public class OrderPosition
    {
        /// <summary> *Уникальный идентификатор товарной позиции в системе </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        /// <summary> *Уникальный идентификатор товара в системе </summary>
        public uint ProductId { get; private set; }

        /// <summary> Артикул производителя (при наличии) </summary>
        public string? ArticleNumber { get; set; } = default!;

        /// <summary> Производитель (бренд) </summary>
        public string Brand { get; set; } = default!;

        /// <summary> *Название товара </summary>
        public string Name { get; set; } = default!;

        /// <summary> Параметры товара </summary>
        public string Params { get; set; } = default!;

        /// <summary> Цена за единицу измерения </summary>
        public decimal Price { get; set; }

        /// <summary> Количество, единиц измерения </summary>
        public float Quantity { get; set; }

        /// <summary> Стоимость общая по товарной позиции </summary>
        public decimal Cost { get; set; }

        /// <summary> Валюта </summary>
        public string Currency { get; set; } = default!;
    }
}
