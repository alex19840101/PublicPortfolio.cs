using System.Collections.Generic;

namespace Orders.API.Contracts
{
    /// <summary> Товарная позиция в заказе </summary>
    public class ProductData
    {
        /// <summary> *Уникальный идентификатор товара в системе </summary>
        public uint Id { get; set; }

        /// <summary> Артикул производителя (при наличии) </summary>
        public string ArticleNumber { get; set; }

        /// <summary> Производитель (бренд) </summary>
        public string Brand { get; set; }

        /// <summary> *Название товара </summary>
        public string Name { get; set; }

        /// <summary> Параметры товара </summary>
        public string Params { get; set; }
        
        /// <summary> Цена за единицу измерения </summary>
        public decimal Price { get; set; }

        /// <summary> Количество, единиц измерения </summary>
        public float Quantity { get; set; }

        /// <summary> Стоимость общая по товарной позиции </summary>
        public decimal Cost { get; set; }

        /// <summary> Валюта </summary>
        public string Currency { get; set; }
    }
}
