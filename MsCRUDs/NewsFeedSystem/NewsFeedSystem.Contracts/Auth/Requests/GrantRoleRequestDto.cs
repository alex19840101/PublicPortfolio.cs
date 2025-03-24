namespace NewsFeedSystem.API.Contracts.Auth.Requests
{
    /// <summary>
    /// Запрос на предоставление роли аккаунту
    /// </summary>
    public class GrantRoleRequestDto
    {
        /// <summary>
        /// Id пользователя*
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) пользователя*
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string NewRole { get; set; }

        /// <summary>
        /// Id администратора*
        /// </summary>
        public uint GranterId { get; set; }

        /// <summary>
        /// Логин (уникальное имя аккаунта) администратора*
        /// </summary>
        public string GranterLogin { get; set; }

        /// <summary>
        /// Пароль администратора
        /// </summary>
        public string Password { get; set; }
    }
}
