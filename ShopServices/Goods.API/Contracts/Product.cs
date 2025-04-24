using System;
using System.Collections.Generic;

namespace Goods.API.Contracts
{
    /// <summary> Товар </summary>
    public class Product
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
        
        /// <summary> Ссылка </summary>
        public string Url { get; set; }
        
        /// <summary> Ссылка на фото/изображение товара </summary>
        public string ImageUrl { get; set; }

        /// <summary> Id групп товаров </summary>
        public List<uint> GoodsGroups { get; set; }

        /// <summary> Дата и время создания данных о товаре </summary>
        public DateTime Created { get; set; }

        /// <summary> Дата и время обновления данных о товаре (опционально) </summary>
        public DateTime? Updated { get; set; }

        /// <summary> В архиве ли товар </summary>
        public bool Archieved { get; set; }
    }
}
