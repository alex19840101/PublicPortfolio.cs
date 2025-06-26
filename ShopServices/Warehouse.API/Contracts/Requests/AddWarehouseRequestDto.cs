namespace Warehouses.API.Contracts.Requests
{
    /// <summary> Запрос на добавление склада </summary>
    public class AddWarehouseRequestDto
    {
        /// <summary> *Название склада </summary>
        public string Name { get; set; } = default!;

        /// <summary> *Код города/населенного пункта </summary>
        public uint RegionCode { get; private set; } = default!;

        /// <summary> *Адрес склада </summary>
        public string Address { get; set; } = default!;

        /// <summary> *Телефон(ы) склада </summary>
        public string Phone { get; set; } = default!;
        
        /// <summary> *E-mail(ы) склада </summary>
        public string Email { get; set; } = default!;
        /// <summary> *URL склада </summary>
        public string Url { get; set; } = default!;

        /// <summary> *Режим работы склада </summary>
        public string WorkSchedule { get; set; } = default!;
    }
}
