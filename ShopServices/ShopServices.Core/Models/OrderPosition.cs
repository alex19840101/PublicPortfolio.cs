using System.Collections.Generic;

namespace ShopServices.Core.Models
{
    /// <summary> Товарная позиция в заказе </summary>
    public class OrderPosition
    {
        /// <summary> *Уникальный идентификатор товарной позиции в системе </summary>
        public uint Id { get; private set; }

        /// <summary> *Уникальный идентификатор товара в системе </summary>
        public uint ProductId { get; private set; }

        /// <summary> Артикул производителя (при наличии) </summary>
        public string ArticleNumber { get; private set; }

        /// <summary> Производитель (бренд) </summary>
        public string Brand { get; private set; }

        /// <summary> *Название товара </summary>
        public string Name { get; private set; }

        /// <summary> Параметры товара </summary>
        public string Params { get; private set; }
        
        /// <summary> Цена за единицу измерения </summary>
        public decimal Price { get; private set; }

        /// <summary> Количество, единиц измерения </summary>
        public float Quantity { get; private set; }

        /// <summary> Стоимость общая по товарной позиции </summary>
        public decimal Cost { get; private set; }

        /// <summary> Валюта </summary>
        public string Currency { get; private set; }
    }
}
