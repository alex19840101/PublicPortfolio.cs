using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    /// <summary> Товарная позиция в заказе </summary>
    public class OrderPosition
    {
        /// <summary> *Уникальный идентификатор товарной позиции в системе </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
        public uint Id { get; private set; }

        /// <summary> Id заказа </summary>
        public uint OrderId { get; private set; }
        
        /// <summary> Навигационное свойство: заказ </summary>
        public Order Order { get; } = default!;

        /// <summary> *Уникальный идентификатор товара в системе </summary>
        public uint ProductId { get; private set; }

        /// <summary> Артикул производителя </summary>
        public string? ArticleNumber { get; private set; } = default!;

        /// <summary> Производитель (бренд) </summary>
        public string Brand { get; private set; } = default!;

        /// <summary> *Название товара </summary>
        public string Name { get; private set; } = default!;

        /// <summary> Параметры товара </summary>
        public string Params { get; private set; } = default!;

        /// <summary> Цена за единицу измерения </summary>
        public decimal Price { get; private set; }

        /// <summary> Количество, единиц измерения </summary>
        public float Quantity { get; private set; }

        /// <summary> Стоимость общая по товарной позиции </summary>
        public decimal Cost { get; private set; }

        /// <summary> Валюта </summary>
        public string Currency { get; private set; } = default!;

        public OrderPosition(
            uint id,
            uint orderId,
            uint productId,
            string articleNumber,
            string brand,
            string name,
            string @params,
            decimal price,
            float quantity,
            decimal cost,
            string currency)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            ArticleNumber = articleNumber;
            Brand = brand;
            Name = name;
            Params = @params;
            Price = price;
            Quantity = quantity;
            Cost = cost;
            Currency = currency;
        }
    }
}
