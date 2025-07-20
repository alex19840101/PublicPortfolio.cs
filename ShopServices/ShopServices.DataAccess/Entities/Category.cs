using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopServices.DataAccess.Entities
{
    public class Category
    {
        /// <summary> *Уникальный идентификатор категории (группы) товаров в системе </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
        public uint Id { get; private set; }

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
        public bool Archieved { get; set; }
    }
}
