using System.Collections.Generic;

namespace ShopServices.Core.Models
{
    /// <summary>
    /// Товар
    /// </summary>
    public class Product
    {
        /// <summary> Уникальный идентификатор товара в системе </summary>
        public uint Id { get; private set; }

        /// <summary> Артикул производителя (при наличии) </summary>
        public string ArticleNumber { get; private set; }

        /// <summary> Производитель (бренд) </summary>
        public string Brand { get; private set; }

        /// <summary> Название товара </summary>
        public string Name { get; private set; }

        /// <summary> Параметры товара </summary>
        public string Params { get; private set; }
        
        /// <summary> Ссылка </summary>
        public string Url { get; private set; }
        
        /// <summary> Ссылка на фото/изображение товара </summary>
        public string ImageUrl { get; private set; }

        /// <summary> Id групп товаров </summary>
        public List<uint> GoodsGroups { get; private set; }

        /// <summary> В архиве ли товар </summary>
        public bool Archieved { get; private set; }

        public Product(uint id,
            string articleNumber,
            string brand,
            string name,
            string parameters,
            string url,
            string imageUrl,
            bool archieved)
        {
            Id = id;
            ArticleNumber = articleNumber;
            Brand = brand;
            Name = name;
            Params = parameters;
            Url = url;
            ImageUrl = imageUrl;
            Archieved = archieved;
        }
    }
}
