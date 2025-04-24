namespace Auth.API.Contracts.Requests
{
    /// <summary>
    /// Запрос на регистрацию аккаунта
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public required string Login { get; set; }

        /// <summary>
        /// Имя (ФИО) пользователя*
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Адрес электронной почты (уникальный)*
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Пароль для входа*
        /// </summary>
        public required string Password { get; set; }
        
        /// <summary>
        /// Пароль для входа (повторно)*
        /// </summary>
        public required string RepeatPassword { get; set; }

        /// <summary>
        /// Ник
        /// </summary>
        public string? Nick { get; set; }

        /// <summary>
        /// Телефон(ы)
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        public string? RequestedRole { get; set; }
    }
}
