using System;

namespace Shops.API.Contracts.Responses
{
    /// <summary> Информация о магазине </summary>
    public class ShopResponseDto
    {
        /// <summary> *Id магазина </summary>
        public uint Id { get; set; }
        
        /// <summary> *Название магазина </summary>
        public string Name { get; set; } = default!;

        /// <summary> Код города/населенного пункта </summary>
        public uint RegionCode { get; set; } = default!;

        /// <summary> *Адрес магазина </summary>
        public string Address { get; set; } = default!;

        /// <summary> *Телефон(ы) магазина </summary>
        public string Phone { get; set; } = default!;
        
        /// <summary> *E-mail(ы) магазина </summary>
        public string Email { get; set; } = default!;
        /// <summary> *URL магазина </summary>
        public string Url { get; set; } = default!;

        /// <summary> *Дата и время создания записи в БД </summary>
        public DateTime CreatedDt { get; set; }

        /// <summary> Дата и время обновления записи в БД (опционально) </summary>
        public DateTime? UpdatedDt { get; set; }

        /// <summary> *Режим работы магазина </summary>
        public string WorkSchedule { get; set; } = default!;
    }
}
