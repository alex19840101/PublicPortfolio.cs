using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    /// <summary>
    /// Товар
    /// </summary>
    public class Product
    {
        /// <summary> Уникальный идентификатор товара в системе </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
        public uint Id { get; private set; }

        /// <summary> Артикул производителя </summary>
        public string ArticleNumber { get; private set; }

        /// <summary> Производитель (бренд) </summary>
        public string Brand { get; private set; }

        /// <summary> Название товара </summary>
        public string Name { get; private set; }

        /// <summary> Параметры товара </summary>
        public string Parameters { get; private set; }
        
        /// <summary> Ссылка </summary>
        public string Url { get; private set; }
        
        /// <summary> Ссылка на фото/изображение товара </summary>
        public string ImageUrl { get; private set; }

        /// <summary> Id групп товаров </summary>
        [Column(TypeName = "integer[]")]
        public List<uint> GoodsGroups { get; private set; }

        /// <summary> Дата и время создания данных о товаре </summary>
        public DateTime Created { get; private set; }

        /// <summary> Дата и время обновления данных о товаре (опционально) </summary>
        public DateTime? Updated { get; private set; }

        /// <summary> В архиве ли товар </summary>
        public bool Archieved { get; private set; }

        /// <summary> Масса, г </summary>
        [Column(TypeName = "integer")]
        public uint MassInGrams { get; private set; } = default!;

        /// <summary> Габариты </summary>
        public string Dimensions { get; private set; } = default!;
        /// <summary> Навигационное свойство: коллекция заказов с данным продуктом </summary>

        public ICollection<Order>? Orders { get; private set; }
        
        /// <summary> Id текущей цены на товар (из Prices) </summary>
        [Column(TypeName = "integer")]
        public uint? PriceId { get; private set; }

        /// <summary> Текущая цена за единицу товара </summary>
        public decimal? PricePerUnit { get; private set; }

        public Product(uint id,
            string articleNumber,
            string brand,
            string name,
            string parameters,
            string url,
            string imageUrl,
            List<uint> goodsGroups,
            bool archieved,
            uint massInGrams,
            string dimensions,
            DateTime created,
            DateTime? updated)
        {
            Id = id;
            ArticleNumber = articleNumber;
            Brand = brand;
            Name = name;
            Parameters = parameters;
            Url = url;
            ImageUrl = imageUrl;
            GoodsGroups = goodsGroups;
            Archieved = archieved;
            MassInGrams = massInGrams;
            Dimensions = dimensions;
            Created = created;
            Updated = updated;
        }

        public void UpdateUpdatedDt(DateTime? updatedDt) => Updated = updatedDt;
        public void UpdateName(string newName) => Name = newName;
        public void UpdateBrand(string newBrand) => Brand = newBrand;
        public void UpdateParameters(string newParameters) => Parameters = newParameters;
        public void UpdateUrl(string newUrl) => Url = newUrl;
        public void UpdateImageUrl(string newImageUrl) => ImageUrl = newImageUrl;
        public void UpdateArchived(bool newArchived) => Archieved = newArchived;
        public void UpdateGoodsGroups(List<uint> newGoodsGroups) => GoodsGroups = newGoodsGroups;
        public void UpdateMassInGrams(uint newMassInGrams) => MassInGrams = newMassInGrams;
        public void UpdateDimensions(string newDimensions) => Dimensions = newDimensions;

        public void UpdatePriceId(uint? newPriceId) => PriceId = newPriceId;
        public void UpdatePricePerUnit(decimal? newPricePerUnit) => PricePerUnit = newPricePerUnit;
    }
}
