namespace NewsFeedSystem.API.Contracts.Auth.Requests
{
    /// <summary>
    /// Запрос на регистрацию аккаунта
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Имя (ФИО) пользователя*
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Адрес электронной почты (уникальный)*
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль для входа*
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Пароль для входа (повторно)*
        /// </summary>
        public string RepeatPassword { get; set; }

        /// <summary>
        /// Ник
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// Телефон(ы)
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        public string RequestedRole { get; set; }
    }
}
