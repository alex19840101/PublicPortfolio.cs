using System;
using System.Collections.Generic;

namespace Goods.API.Contracts
{
    /// <summary> Товар </summary>
    public class Product
    {
        /// <summary> *Уникальный идентификатор товара в системе </summary>
        public uint Id { get; set; }

        /// <summary> Артикул производителя </summary>
        public string ArticleNumber { get; set; } = default!;

        /// <summary> Производитель (бренд) </summary>
        public string Brand { get; set; } = default!;

        /// <summary> *Название товара </summary>
        public string Name { get; set; } = default!;

        /// <summary> Параметры товара </summary>
        public string Params { get; set; } = default!;

        /// <summary> Ссылка </summary>
        public string Url { get; set; } = default!;

        /// <summary> Ссылка на фото/изображение товара </summary>
        public string ImageUrl { get; set; } = default!;

        /// <summary> Id групп товаров </summary>
        public List<uint> GoodsGroups { get; set; } = null!;

        /// <summary> Дата и время создания данных о товаре </summary>
        public DateTime Created { get; set; }

        /// <summary> Дата и время обновления данных о товаре (опционально) </summary>
        public DateTime? Updated { get; set; }

        /// <summary> В архиве ли товар </summary>
        public bool Archived { get; set; } = false;

        /// <summary> Масса, г </summary>
        public uint MassInGrams { get; set; }

        /// <summary> Габариты </summary>
        public string Dimensions { get; set; } = default!;

        /// <summary> Id текущей цены на товар. Изменение цены методом UpdateProduct
        /// <para> Создание ценника - через Prices.API/AddPrice </para></summary>
        public uint? PriceId { get; set; }

        /// <summary> Текущая цена за единицу товара. Изменение цены методом UpdateProduct
        /// <para> Создание ценника - через Prices.API/AddPrice </para></summary>
        public decimal? PricePerUnit { get; set; }
    }
}
