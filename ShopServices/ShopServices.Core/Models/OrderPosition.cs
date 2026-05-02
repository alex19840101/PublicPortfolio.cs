using System;
using System.Text.Json.Serialization;

namespace ShopServices.Core.Models
{
    /// <summary> Товарная позиция в заказе </summary>
    [JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)] //для десериализации свойства только для чтения: https://learn.microsoft.com/ru-ru/dotnet/standard/serialization/system-text-json/immutability
    public class OrderPosition
    {
        /// <summary> *Уникальный идентификатор товарной позиции в системе </summary>
        [JsonInclude]  public uint Id { get; private set; }

        /// <summary> *Уникальный идентификатор товара в системе </summary>
        [JsonInclude] public uint ProductId { get; private set; }

        /// <summary> Артикул производителя (при наличии) </summary>
        [JsonInclude] public string ArticleNumber { get; private set; }

        /// <summary> Производитель (бренд) </summary>
        [JsonInclude] public string Brand { get; private set; }

        /// <summary> *Название товара </summary>
        [JsonInclude] public string Name { get; private set; }

        /// <summary> Параметры товара </summary>
        [JsonInclude] public string Params { get; private set; }

        /// <summary> Цена за единицу измерения </summary>
        [JsonInclude] public decimal Price { get; private set; }

        /// <summary> Количество, единиц измерения </summary>
        [JsonInclude] public float Quantity { get; private set; }

        /// <summary> Стоимость общая по товарной позиции </summary>
        [JsonInclude] public decimal Cost { get; private set; }

        /// <summary> Валюта </summary>
        [JsonInclude] public string Currency { get; private set; }

        [JsonConstructor]
        public OrderPosition(
            uint id,
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

        /// <summary>
        /// Заполнение полей товарной позиции данными товара
        /// </summary>
        /// <param name="product"> Product - товар </param>
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
