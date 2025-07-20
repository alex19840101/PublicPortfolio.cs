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
        public bool Archived { get; private set; }
        
        /// <summary> Масса, г </summary>
        public uint MassInGrams { get; private set; }

        /// <summary> Габариты </summary>
        public string Dimensions { get; private set; }
        
        /// <summary> Id текущей цены на товар (из Prices) </summary>
        public uint? PriceId { get; private set; }
        
        /// <summary> Текущая цена за единицу измерения товара </summary>
        public decimal? PricePerUnit { get; private set; }


        public Product(uint id,
            string articleNumber,
            string brand,
            string name,
            string parameters,
            string url,
            string imageUrl,
            List<uint> goodsGroups,
            bool archived,
            uint massInGrams,
            string dimensions,
            uint? priceId,
            decimal? pricePerUnit,
            DateTime? created,
            DateTime? updated)
        {
            Id = id;
            ArticleNumber = articleNumber;
            Brand = brand;
            Name = name;
            Params = parameters;
            Url = url;
            ImageUrl = imageUrl;
            GoodsGroups = goodsGroups;
            Archived = archived;
            MassInGrams = massInGrams;
            Dimensions = dimensions;
            PriceId = priceId;
            PricePerUnit = pricePerUnit;
            if (created != null)
                Created = created.Value;
            Updated = updated;
        }

        /// <summary>
        /// Проверка на равенство (существующей Product) с игнорированием:
        /// <para> - Id (Id до момента регистрации не определен, как бы равен нулю),</para>
        /// <para> - Created, Updated, Archived - не важны для сравнения</para>
        /// <para> - GoodsGroups (для упрощения), PriceId, PricePerUnit </para>
        /// </summary>
        /// <param name="comparedProduct"> Product для сравнения</param>
        /// <returns></returns>
        public bool IsEqualIgnoreIdAndDt(Product comparedProduct)
        {
            if (!string.Equals(comparedProduct.Name, Name) ||
                !string.Equals(comparedProduct.ArticleNumber, ArticleNumber) ||
                !string.Equals(comparedProduct.Brand, Brand) ||
                !string.Equals(comparedProduct.Params, Params) ||
                !string.Equals(comparedProduct.Url, Url) ||
                !string.Equals(comparedProduct.ImageUrl, ImageUrl) ||
                !string.Equals(comparedProduct.Dimensions, Dimensions) ||
                comparedProduct.MassInGrams != MassInGrams)
                    return false;

            return true;
        }
    }
}
