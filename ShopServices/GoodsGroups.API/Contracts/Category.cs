using System;

namespace GoodsGroups.API.Contracts
{
    /// <summary> Категория (группа) товаров в системе </summary>
    public class Category
    {
        /// <summary> *Уникальный идентификатор категории (группы) товаров в системе
        /// <para> При добавлении новой категории указывать 0 </para></summary>
        public uint Id { get; set; }

        /// <summary> *Название категории (группы) товаров </summary>
        public string Name { get; set; } = default!;

        /// <summary> Производитель (бренд) (опционально) </summary>
        public string? Brand { get; set; }

        /// <summary> Параметры (опционально) </summary>
        public string? Params { get; set; }

        /// <summary> Ссылка (опционально) </summary>
        public string? Url { get; set; }

        /// <summary> Ссылка на фото/изображение категории (опционально) </summary>
        public string? ImageUrl { get; set; }

        /// <summary> Дата и время создания категории </summary>
        public DateTime Created { get; set; }

        /// <summary> Дата и время обновления категории (опционально) </summary>
        public DateTime? Updated { get; set; }

        /// <summary> В архиве ли категория </summary>
        public bool Archieved { get; set; } = false;
    }
}
