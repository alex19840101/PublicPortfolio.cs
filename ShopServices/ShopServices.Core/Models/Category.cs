using System;

namespace ShopServices.Core.Models
{
    /// <summary> Категория (группа) товаров в системе </summary>
    public class Category
    {
        /// <summary> *Уникальный идентификатор категории (группы) товаров в системе </summary>
        public uint Id { get; private set; }

        /// <summary> *Название категории (группы) товаров </summary>
        public string Name { get; private set; }

        /// <summary> Производитель (бренд) (опционально) </summary>
        public string Brand { get; private set; }

        /// <summary> Параметры (опционально) </summary>
        public string Params { get; private set; }

        /// <summary> Ссылка (опционально) </summary>
        public string Url { get; private set; }

        /// <summary> Ссылка на фото/изображение категории (опционально) </summary>
        public string ImageUrl { get; private set; }

        /// <summary> Дата и время создания категории </summary>
        public DateTime? Created { get; private set; }

        /// <summary> Дата и время обновления категории (опционально) </summary>
        public DateTime? Updated { get; private set; }

        /// <summary> В архиве ли категория </summary>
        public bool Archived { get; private set; }

        public Category(uint id,
            string name,
            string brand = null,
            string url = null,
            string imageUrl = null,
            string parameters = null,
            bool archived = false,
            DateTime? created = null,
            DateTime? updated = null)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Url = url;
            ImageUrl = imageUrl;
            Params = parameters;
            Archived = archived;
            Created = created;
            Updated = updated;
        }

        /// <summary>
        /// Проверка на равенство (существующей Category) с игнорированием:
        /// <para> - Id (Id до момента регистрации не определен, как бы равен нулю),</para>
        /// <para> - Created, Updated, Archived - не важны для сравнения</para>
        /// </summary>
        /// <param name="comparedCategory"> Category для сравнения</param>
        /// <returns></returns>
        public bool IsEqualIgnoreIdAndDt(Category comparedCategory)
        {
            if (!string.Equals(comparedCategory.Name, Name) ||
                !string.Equals(comparedCategory.Brand, Brand) ||
                !string.Equals(comparedCategory.Params, Params) ||
                !string.Equals(comparedCategory.Url, Url) ||
                !string.Equals(comparedCategory.ImageUrl, ImageUrl))
                return false;

            return true;
        }
    }
}
