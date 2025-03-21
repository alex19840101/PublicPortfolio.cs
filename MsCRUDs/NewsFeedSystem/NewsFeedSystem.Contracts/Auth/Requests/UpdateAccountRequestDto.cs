namespace NewsFeedSystem.API.Contracts.Auth.Requests
{
    /// <summary>
    /// Запрос на обновление аккаунта
    /// </summary>
    public class UpdateAccountRequestDto
    {
        /// <summary>
        /// Id пользователя*
        /// </summary>
        public int Id { get; set; }

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
        /// Существующий пароль*
        /// </summary>
        public string ExistingPassword { get; set; }

        /// <summary>
        /// Новый пароль (если нужна смена пароля)
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// Новый пароль (повторно)(если нужна смена пароля)
        /// </summary>
        public string RepeatNewPassword { get; set; }

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
