namespace Shops.API.Contracts.Requests
{
    /// <summary> Запрос на добавление магазина </summary>
    public class AddShopRequestDto
    {
        /// <summary> *Название магазина </summary>
        public string Name { get; set; } = default!;
        
        /// <summary> *Адрес магазина </summary>
        public string Address { get; set; } = default!;

        /// <summary> *Телефон(ы) магазина </summary>
        public string Phone { get; set; } = default!;
        
        /// <summary> *E-mail(ы) магазина </summary>
        public string Email { get; set; } = default!;
        /// <summary> *URL магазина </summary>
        public string Url { get; set; } = default!;
    }
}
