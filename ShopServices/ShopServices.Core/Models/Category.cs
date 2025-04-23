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

        /// <summary> В архиве ли товар </summary>
        public bool Archieved { get; private set; }

        public Category(uint id,
            string name,
            string brand = null,
            string url = null,
            string imageUrl = null,
            string parameters = null,
            bool archieved = false)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Url = url;
            ImageUrl = imageUrl;
            Params = parameters;
            Archieved = archieved;
        }
    }
}
