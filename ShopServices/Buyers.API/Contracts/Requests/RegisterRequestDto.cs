namespace Buyers.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на регистрацию аккаунта покупателя
    /// </summary>
    public class RegisterRequestDto
    {
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
        public string Address { get; set; }

        /// <summary>
        /// Адрес электронной почты (уникальный) покупателя*
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Пароль для входа покупателя*
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Пароль для входа покупателя (повторно)*
        /// </summary>
        public required string RepeatPassword { get; set; }

        /// <summary>
        /// Ник покупателя
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// Телефон(ы) покупателя
        /// </summary>
        public string Phone { get; set; }
    }
}
