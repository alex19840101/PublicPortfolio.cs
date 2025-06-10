namespace Buyers.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на обновление аккаунта покупателя
    /// </summary>
    public class UpdateAccountRequestDto
    {
        /// <summary>
        /// Id покупателя*
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) покупателя*
        /// </summary>
        public required string Login { get; set; }

        /// <summary>
        /// Имя [, отчество] покупателя*
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Фамилия покупателя*
        /// </summary>
        public required string Surname { get; set; }

        /// <summary>
        /// Адрес доставки по умолчанию для покупателя (опционально)
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Адрес электронной почты (уникальный)*
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Существующий пароль*
        /// </summary>
        public required string ExistingPassword { get; set; }

        /// <summary>
        /// Новый пароль (если нужна смена пароля)
        /// </summary>
        public string? NewPassword { get; set; }

        /// <summary>
        /// Новый пароль (повторно)(если нужна смена пароля)
        /// </summary>
        public string? RepeatNewPassword { get; set; }

        /// <summary>
        /// Ник
        /// </summary>
        public string? Nick { get; set; }

        /// <summary>
        /// Телефон(ы)
        /// </summary>
        public string? Phone { get; set; }
    }
}
