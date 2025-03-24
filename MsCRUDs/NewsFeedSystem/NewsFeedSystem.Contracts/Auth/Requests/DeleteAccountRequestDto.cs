namespace NewsFeedSystem.API.Contracts.Auth.Requests
{
    /// <summary>
    /// Запрос на удаление аккаунта
    /// </summary>
    public class DeleteAccountRequestDto
    {
        /// <summary>
        /// Id пользователя*
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль*
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Пароль (повторно)*
        /// </summary>
        public string RepeatPassword { get; set; }

        /// <summary>
        /// Id администратора (если удаление администратором)
        /// </summary>
        public uint? GranterId { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) администратора (если удаление администратором)*
        /// </summary>
        public string GranterLogin { get; set; }
    }
}
