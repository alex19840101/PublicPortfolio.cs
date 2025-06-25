namespace Warehouses.API.Contracts.Responses
{
    /// <summary> Информация о складе </summary>
    public class WarehousepResponseDto
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
    }
}
