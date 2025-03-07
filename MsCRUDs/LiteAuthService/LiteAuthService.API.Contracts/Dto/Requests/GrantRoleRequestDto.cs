namespace LiteAuthService.API.Contracts.Dto.Requests.Auth
{
    /// <summary>
    /// Запрос на предоставление роли аккаунту
    /// </summary>
    public class GrantRoleRequestDto
    {
        /// <summary>
        /// Id пользователя*
        /// </summary>
        public int Id { get; set; }

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
        public int GranterId { get; set; }

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
