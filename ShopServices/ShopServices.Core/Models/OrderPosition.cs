using System;

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

        public OrderPosition(
            uint id,
            uint productId,
            string articleNumber,
            string brand,
            string name,
            string parameters,
            decimal price,
            float quantity,
            decimal cost,
            string currency)
        {
            Id = id;
            ProductId = productId;
            ArticleNumber = articleNumber;
            Brand = brand;
            Name = name;
            Params = parameters;
            Price = price;
            Quantity = quantity;
            Cost = cost;
            Currency = currency;
        }

        /// <summary>
        /// Заполнение полей товарной позиции данными продукта
        /// </summary>
        /// <param name="product"></param>
        public void FillByProduct(Product product)
        {
            if (product.Id != ProductId)
                throw new InvalidOperationException($"{ResultMessager.PRODUCT_ID_MISMATCH}: product.Id={product.Id} != OrderPosition.ProductId={ProductId}");
            
            ArticleNumber = product.ArticleNumber;
            Brand = product.Brand;
            Name = product.Name;
            Params = product.Params;
        }
    }
}
