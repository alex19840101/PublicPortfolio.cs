using System;

namespace Warehouses.API.Contracts.Responses
{
    /// <summary> Информация о складе </summary>
    public class WarehouseResponseDto
    {
        /// <summary> *Id склада </summary>
        public uint Id { get; set; }
        
        /// <summary> *Название склада </summary>
        public string Name { get; set; } = default!;
        
        /// <summary> *Адрес склада </summary>
        public string Address { get; set; } = default!;

        /// <summary> *Телефон(ы) склада </summary>
        public string Phone { get; set; } = default!;
        
        /// <summary> *E-mail(ы) склада </summary>
        public string Email { get; set; } = default!;
        /// <summary> *URL склада </summary>
        public string Url { get; set; } = default!;

        /// <summary> Дата и время создания записи в БД </summary>
        public DateTime CreatedDt { get; set; }

        /// <summary> Дата и время обновления записи в БД (опционально) </summary>
        public DateTime? UpdatedDt { get; set; }
    }
}
