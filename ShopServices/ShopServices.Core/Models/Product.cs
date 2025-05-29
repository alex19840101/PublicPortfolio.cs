using System;
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

        /// <summary> Дата и время создания данных о товаре </summary>
        public DateTime Created { get; private set; }

        /// <summary> Дата и время обновления данных о товаре (опционально) </summary>
        public DateTime? Updated { get; private set; }

        /// <summary> В архиве ли товар </summary>
        public bool Archieved { get; private set; }
        
        /// <summary> Масса, г </summary>
        public string MassInGrams { get; private set; }

        /// <summary> Габариты </summary>
        public string Dimensions { get; private set; }


        public Product(uint id,
            string articleNumber,
            string brand,
            string name,
            string parameters,
            string url,
            string imageUrl,
            List<uint> goodsGroups,
            bool archieved,
            string massInGrams,
            string dimensions)
        {
            Id = id;
            ArticleNumber = articleNumber;
            Brand = brand;
            Name = name;
            Params = parameters;
            Url = url;
            ImageUrl = imageUrl;
            GoodsGroups = goodsGroups;
            Archieved = archieved;
            MassInGrams = massInGrams;
            Dimensions = dimensions;
        }
    }
}
